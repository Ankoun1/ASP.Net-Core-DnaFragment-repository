namespace DnaFragment.Services.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DnaFragment.Data;
    using DnaFragment.Data.Models;
    using DnaFragment.Models.Users;
    using DnaFragment.Services.Mail;
    using DnaFragment.Services.Messages;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.WebUtilities;

    public class UsersService : IUsersService
    {
        private readonly DnaFragmentDbContext data; 
        private readonly ISendMailService sendMail;
        private readonly IMessagesService messageService;
        private readonly UserManager<User> _userManager;
      

        public UsersService(DnaFragmentDbContext data, ISendMailService sendMail, UserManager<User> _userManager, IMessagesService messageService)
        {
            this.data = data;
            this.sendMail = sendMail;
            this.messageService = messageService;
            this._userManager = _userManager;
        }

        public void SendmailForgotPassword(string email)
        {
            int r = SerialNumberGenerator();

            var user = ValidEmail(email);

            user.ResetPasswordId = r;
            data.SaveChanges();
            sendMail.SendEmailAsync(user.Id, "Reset Password from DnaFragment", $"Hello {user.UserName},your identification number is {r} Follow the link " +
                $"https://localhost:44350/LrUsers/ResetPassword to reset your password").Wait();
        }

        private static int SerialNumberGenerator()
        {
            Random generator = new Random();
            int r = generator.Next(100000, 1000000);
            return r;
        }

        public async Task ResetPasswordDb(int? code, string password)
        {            
            var user = data.Users.Where(x => x.ResetPasswordId == code).FirstOrDefault();

            await GeneratorPassword(password,user);
        }

        public UserListingViewModel UsersDb(string userId)
        {
            var user = data.Users.Where(x => x.Id == userId).Select(x => new UserListingViewModel
            {
                Id = x.Id,
                Username = x.FullName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                IsAdministrator = x.IsAdministrator,
                NumberMessages = data.Messages.Where(x => x.UserId == userId).Count(),
                NumberQuestions = data.Questions.Where(x => x.UserId == userId).Count(),
                LRPoints = data.LrUsers.Where(y => y.Email == x.Email).Select(y => y.LrPoints).FirstOrDefault()
            }).FirstOrDefault();
           
            return user;
        }

        public bool CodCheck(int? code)
        => this.data.Users.Any(u => u.ResetPasswordId == code);
    
        public void DeleteUsersDb(string userId,bool isAdmin)
        {
            if (UserIsRegister(userId)) 
            {                 
                var user = data.Users.Find(userId);
                if (isAdmin)
                {
                    var lrUser = data.LrUsers.Where(x => x.Email == user.Email).FirstOrDefault();
                    lrUser.IsDanger = true;
                }
                data.Users.Remove(user);
                data.SaveChanges();
            }

            var bagInformation = data.Bags.Where(x => x.UserId == userId).ToList();
            if(bagInformation != null)
            {
                data.Bags.RemoveRange(bagInformation);
                data.SaveChanges();
            }           
        }
        
        public bool UserIsRegister(string userId)
            => this.data
           .Users
           .Any(d => d.Id == userId);

        public void AutomaticDeleteDb()
        {
            DateTime curentdate = DateTime.UtcNow;
            var questionsOld = data.Questions.ToList().Where(x => !x.StopAutomaticDelete && (curentdate - x.CreatedOn).TotalDays > 30.0);
            var answersOld = data.Answers.ToList().Where(x => !x.StopAutomaticDelete && (curentdate - x.CreatedOn).TotalDays > 30);
            var messagesOld = data.Messages.ToList().Where(x => !x.StopAutomaticDelete && (curentdate - x.CreatedOn).TotalDays > 360);

            data.Questions.RemoveRange(questionsOld);
            data.Answers.RemoveRange(answersOld);
            data.Messages.RemoveRange(messagesOld);
            data.SaveChanges();
        }

        public User ValidEmail(string email)
        => data.Users.Where(x => x.Email == email).FirstOrDefault();

        public List<LrUsersStatisticsFormModel> UsersStatistics(byte sort)
        {
            AutomaticDeleteDb();
            var lrUsers = data.LrUsers.OrderBy(x => x.Email).ToList();
            var users = new List<LrUsersStatisticsFormModel>();
            foreach (var lrUser in lrUsers)
            {
                var statisticsProduct = data.LrUserStatisticsProducts.Where(y => y.LrUserId == lrUser.Id).OrderBy(x => x.StatisticsProduct.StatisticsCategoryId).Distinct().ToList();
           
                StringBuilder sb = new StringBuilder();

                if (lrUser.Email != "unknown@city.com")
                {
                    var userDb = data.Users.Where(x => !x.IsAdministrator).Where(x => x.Email == lrUser.Email).Select(x => new { Id = x.Id, FullName = x.FullName, PhoneNumber = x.PhoneNumber ,IsSuperUser = x.IsSuperUser}).FirstOrDefault();
                    

                    if (userDb == null)
                    {
                        var user = new LrUsersStatisticsFormModel
                        {
                            Id = null,
                            Username = "---",
                            Email = lrUser.Email,
                            LrPoints = $"LR: { lrUser.LrPoints }",
                            PhoneNumber = "---",
                            IsDanger = lrUser.IsDanger,
                            CategoriesVisitsCount = GetCategoriesCount(lrUser.Id),
                        };
                        AddUserProductsCount(users, statisticsProduct, sb, user);
                    }
                    else
                    {
                        var user = new LrUsersStatisticsFormModel
                        {
                            Id = userDb.Id,
                            Username = userDb.FullName,
                            Email = lrUser.Email,
                            IsSuperUser = userDb.IsSuperUser,
                            LrPoints = $"LR: { lrUser.LrPoints }",
                            PhoneNumber = $"Tel: {userDb.PhoneNumber}",
                            CategoriesVisitsCount = GetCategoriesCount(lrUser.Id),
                        };
                        AddUserProductsCount(users, statisticsProduct, sb, user);
                    }
                }
                else
                {
                    var user = new LrUsersStatisticsFormModel
                    {
                        Username = "Users not registration",
                        Email = lrUser.Email,
                        LrPoints = "---",
                        PhoneNumber = "---",
                        CategoriesVisitsCount = GetCategoriesCount(lrUser.Id),
                    };
                    AddUserProductsCount(users, statisticsProduct, sb, user);
                }
            }
            return Sorting(sort, ref users);
        }

        private string GetCategoriesCount(int lrUserId)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 1; i <= 7; i++)
            {
                foreach (var item in data.StatisticsProducts.ToList())
                {
                    if (item.StatisticsCategoryId == i)
                    {
                        var categoruVisitsCount = data.LrUserStatisticsProducts.Where(x => x.StatisticsProductId == item.Id && x.LrUserId == lrUserId).Select(x => x.CategoryVisitsCount).FirstOrDefault();
                        builder.Append($"{i} ({categoruVisitsCount}) ");
                        break;
                    }
                }
            }
            return builder.ToString();
        }

        private static List<LrUsersStatisticsFormModel> Sorting(byte sort, ref List<LrUsersStatisticsFormModel> users)
        {
            users = users.OrderByDescending(x => x.Username).ToList();
            if (sort == 1)
            {
                users = users.OrderBy(x => x.Username).ToList();
            }
            else if (sort == 2)
            {
                users = users.OrderBy(x => x.Id).ToList();
            }
            return users;
        }

        private void AddUserProductsCount(List<LrUsersStatisticsFormModel> users, List<LrUserStatisticsProduct> statisticsProduct, StringBuilder sb, LrUsersStatisticsFormModel user)
        {
            foreach (var item in statisticsProduct)
            {
                var lrProduct = data.StatisticsProducts.Where(x => x.Id == item.StatisticsProductId).FirstOrDefault();
                var product = data.LrProducts.Where(x => x.Id == item.StatisticsProductId).FirstOrDefault();
                sb.Append($"<{lrProduct.PlateNumber} ({item.ProductVisitsCount}) $({item.PurchasesCount}) Total({product.TotalPurchasesCount})>");
            }
            user.ProductsVisitsCount = sb.ToString();
            users.Add(user);
        }

        public async Task UpdateDb(string userId, string fullName, string email, string phoneNumber, string password)
        {
            var user = data.Users.Find(userId);                      
            
            var lrUser = data.LrUsers.Where(x => x.Email == user.Email).FirstOrDefault();

            var curentUser =  await CorrectUpdate(lrUser, fullName, email, phoneNumber, password, user);
        }

        public  async Task GeneratorPassword(string password,User user)
        {
            var code1 = await _userManager.GeneratePasswordResetTokenAsync(user);
            var code2 = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code1));
            var Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code2));
            var userReset = await _userManager.FindByEmailAsync(user.Email);
            await _userManager.ResetPasswordAsync(userReset, Code, password);
        }       

        public async Task<User> CorrectUpdate(LrUser lrUser, string fullName,string email, string phoneNumber, string password,User user)
        {
            if (lrUser != null)
            {
                if (!data.LrUsersOldEmails.Any(x => x.LrUserId == lrUser.Id && x.Email == email))
                {
                    var oldEmails = new LrUserOldEmails { Email = lrUser.Email, LrUserId = lrUser.Id };
                    data.LrUsersOldEmails.Add(oldEmails);
                    data.SaveChanges();
                }

                lrUser.Email = email;

                user.FullName = fullName;
                user.UserName = email;
                user.Email = email;
                user.NormalizedEmail = email.ToUpper();
                user.NormalizedUserName = email.ToUpper();

                if (phoneNumber != null)
                {
                    user.PhoneNumber = phoneNumber;
                    data.SaveChanges();
                }

                data.SaveChanges();

                await GeneratorPassword(password, user);
            }
            return user;
        }

        public void AddNewLrUserInfoDb(LrUser lrUser)       
        {         
            var lrUserStatisticsProduct = new List<LrUserStatisticsProduct>();
            for (int i = 1; i <= 7; i++)
            {
                var statisticsProducts = data.StatisticsProducts.Where(x => x.StatisticsCategoryId == i).Select(x => x.Id).ToList();
                foreach (var item in statisticsProducts)
                {
                    lrUserStatisticsProduct.Add(new LrUserStatisticsProduct { LrUserId = lrUser.Id, LrUser = lrUser, StatisticsProductId = item });
                }
            }
            data.LrUserStatisticsProducts.AddRange(lrUserStatisticsProduct);
            data.SaveChanges();
        }

        public (decimal,decimal,bool) Amount(string lrUserId)
        {
            var user = data.Users.Where(x => x.Id == lrUserId).Select(x => new { Amount = x.Amount, PercentageDiscount = x.PercentageDiscount }).FirstOrDefault();
            

            var productBought = data.UserProducts.Where(x => x.UserId == lrUserId && x.Bought).Select(x => x.Bought).FirstOrDefault();

            return (user.Amount,user.PercentageDiscount, productBought);
        }

        public void UpdateUserProducts(string lrUserId, int productId,int productsCount)
        {
            var userProduct = data.UserProducts.Where(x => x.UserId == lrUserId && x.LrProductId == productId).FirstOrDefault();
            userProduct.LrProductsCount = productsCount;
            userProduct.Bought = true;

            var price = data.LrProducts.Where(x => x.Id == userProduct.LrProductId).Select(x => x.Price).FirstOrDefault();
            decimal amount = price * productsCount;
            decimal amountWithDiscount = 0;
            decimal localPercentageDiscount = 1;
            if (IsSuperUser(lrUserId))
            {
                localPercentageDiscount = 0.92m;
            }
            else if (productsCount > 50)
            {
                localPercentageDiscount = 0.87m;
            }
            else if (productsCount > 10)
            {
                localPercentageDiscount = 0.92m;
            }
            else if (productsCount > 5)
            {
                localPercentageDiscount = 0.95m;
            }

            amountWithDiscount = amount * localPercentageDiscount;
            userProduct.Amount = amountWithDiscount;
            userProduct.PercentageDiscount = (1 - localPercentageDiscount) * 100;
            data.SaveChanges();

            UpdateUserTotal(lrUserId);
        }

        public void UpdateUserTotal(string lrUserId)
        {
            var user = data.Users.Where(x => x.Id == lrUserId).FirstOrDefault();
            var localUserCount = user.Count;

            var userProducts = data.UserProducts.Where(x => x.UserId == lrUserId && x.Bought).ToList();
            decimal amounthProducts = userProducts.Select(x => x.Amount).Sum();
            decimal userPercentageDiscount = 0;
            if(userProducts.Any())
            {
               userPercentageDiscount = userProducts.Select(x => x.PercentageDiscount).Average();
            }
            
            var localAmount = amounthProducts + 3.9m;

            if (ResetRevers50Points(lrUserId) && userProducts.Any())
            {
                localAmount -= localUserCount;
                if (localAmount < 0)
                {
                    localUserCount = Math.Abs(localAmount);
                    localAmount = 0;
                }
                else
                {
                    localUserCount = 0;
                }

                if (localAmount > 0)
                {
                    userPercentageDiscount += (user.Count * 100) / amounthProducts;
                }

            }
            user.Amount = localAmount;
            user.PercentageDiscount = userPercentageDiscount;
            user.Count = localUserCount;
            data.SaveChanges();
        }

        public void Order(string lrUserId, string city, string address, string phoneNumber)
        {
            var user = data.Users.Where(x => x.Id == lrUserId).FirstOrDefault();
            int r = SerialNumberGenerator();
            var userBag = new Bag { UserId = lrUserId, City = city, ShippingAddress = address, PhoneNumber = phoneNumber,DeliveryNumber = r, Total = user.Amount };
            data.Bags.Add(userBag);
            data.SaveChanges();

            var products = data.UserProducts.Where(x => x.UserId == lrUserId && x.Bought).ToList();
            var bagProducts = products.Select(x => new BagProduct { BagId = userBag.Id, LrProductId = x.LrProductId,LrProduct = x.LrProduct,CountProducts = x.LrProductsCount }).ToList();
            data.BagProducts.AddRange(bagProducts);

            var subject = "Delivery Information from DnaFragment";
            var body = $"Благодарим ви за доверието!Вие избрахте висококачествените немски продукти,LR иновации за дълголетие.Вашата поръчка е потвърдена! Номер за идентификация на пратката {r}.";


            sendMail.SendEmailAsync(lrUserId, subject, body).Wait();

            var lrUser = data.LrUsers.Where(x => x.Email == user.Email).FirstOrDefault();
            if(user.Amount > 0)
            {
                lrUser.LrPoints += (int)user.Amount / 30;
                lrUser.Reverse50Points += (int)user.Amount / 30;
            }           
            user.Amount = 0;
            user.PercentageDiscount = 0;
            data.SaveChanges();

           

            foreach (var product in products)
            {
                var p = data.LrProducts.Where(x => x.Id == product.LrProductId).FirstOrDefault();     
                var lrUserStatisticsProduct = data.LrUserStatisticsProducts.Where(x => x.LrUserId == lrUser.Id && x.StatisticsProductId == product.LrProductId).FirstOrDefault();
                lrUserStatisticsProduct.PurchasesCount += product.LrProductsCount;
                lrUser.LrPoints += product.LrProductsCount;               
                p.TotalPurchasesCount += product.LrProductsCount;               
                product.Bought = false;
                product.LrProductsCount = 0;
                data.SaveChanges();
            }
            if(lrUser.LrPoints > 500)
            {
                user.IsSuperUser = true;
                data.SaveChanges();
            }            
        }

        public bool IsSuperUser(string lrUserId)
        => data.Users.Any(x => x.Id == lrUserId && x.IsSuperUser);

        public List<UserOrderFormModel> ShippingOrders(string lrUserId)
        {
            var userInfoOrders = data.Bags.Where(x => x.UserId == lrUserId && !x.IsSent).ToList()
            .Select(x => new UserOrderFormModel {Total = x.Total,City = x.City,ShippingAddress = x.ShippingAddress,PhoneNumber = x.PhoneNumber,DeliveryNumber = x.DeliveryNumber,BagId = x.Id }).ToList();
           
            foreach (var order in userInfoOrders)
            {
                StringBuilder sb = new StringBuilder();               
                foreach (var item in data.BagProducts.Where(x => x.BagId == order.BagId).ToList())
                {
                    var productNumber = data.LrProducts.Where(x => x.Id == item.LrProductId).Select(x => x.PlateNumber).FirstOrDefault();
                    sb.Append($"P: <PN: ({productNumber}) Cp: ({item.CountProducts})> ");
                }
                order.BagProducts = sb.ToString();
                order.FullName = data.Users.Where(x => x.Id == lrUserId).Select(x => x.FullName).FirstOrDefault();
            }

            return userInfoOrders;           
        }

        public void Received(int bagId)
        {
            var bag = data.Bags.Find(bagId);
            bag.IsSent = true;
            data.SaveChanges();
        }

        public string GetPhoneNumber(string userId)
        => data.Users.Where(x => x.Id == userId).Select(x => x.PhoneNumber).FirstOrDefault();

        public bool ResetRevers50Points(string userId)
        {
            bool reset = false;
            var user = data.Users.Find(userId);
            var lrUser = data.LrUsers.Where(x => x.Email == user.Email && x.Reverse50Points >= 15).FirstOrDefault();
            if (lrUser != null)
            {
                messageService.AddMessageDb(userId, "Вашите LR points се увеличиха с 50.Печелите продукти по избор на обща стойност 60 лв.");
                lrUser.Reverse50Points = 0;
                user.Count = 60;
                reset = true;
            }
            return reset;
        }        
    }
}
