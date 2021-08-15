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
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.WebUtilities;

    public class UsersService : IUsersService
    {
        private readonly DnaFragmentDbContext data; 
        private readonly ISendMailService sendMail;
        private readonly UserManager<User> _userManager;

        public UsersService(DnaFragmentDbContext data, ISendMailService sendMail, UserManager<User> _userManager)
        {
            this.data = data;
            this.sendMail = sendMail;
            this._userManager = _userManager;
        }

        public void SendmailForgotPassword(string email)
        {
            Random generator = new Random();
            int r = generator.Next(100000, 1000000);

            var user = ValidEmail(email);
            
                user.ResetPasswordId = r;
                data.SaveChanges();
                sendMail.SendEmailAsync(user.Id, "Reset Password from DnaFragment", $"Hello {user.UserName},your identification number is {r} Follow the link " +
                    $"https://localhost:44350/LrUsers/ResetPassword to reset your password").Wait();            
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
                NumberQuestions = data.Questions.Where(x => x.UserId == userId).Count()
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


                var statisticsProduct = data.LrUserStatisticsProducts.Where(y => y.LrUserId == lrUser.Id).ToList();
                StringBuilder sb = new StringBuilder();

                if (lrUser.Email != "unknown@city.com")
                {
                    var userDb = data.Users.Where(x => !x.IsAdministrator).Where(x => x.Email == lrUser.Email).Select(x => new { Id = x.Id, FullName = x.FullName, PhoneNumber = x.PhoneNumber }).FirstOrDefault();
                    if (userDb == null)
                    {
                        var user = new LrUsersStatisticsFormModel
                        {
                            Id = null,
                            Username = "---",
                            Email = lrUser.Email,
                            PhoneNumber = "---",
                            IsDanger = lrUser.IsDanger,
                            CategoryVisitsCount = statisticsProduct[0].CategoryVisitsCount,
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
                            PhoneNumber = userDb.PhoneNumber,
                            CategoryVisitsCount = statisticsProduct[0].CategoryVisitsCount,
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
                        PhoneNumber = "---",
                        CategoryVisitsCount = statisticsProduct[0].CategoryVisitsCount,
                    };
                    AddUserProductsCount(users, statisticsProduct, sb, user);
                }
            }
            return Sorting(sort, ref users);
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
                var product = data.StatisticsProducts.Where(x => x.Id == item.StatisticsProductId).FirstOrDefault();
                sb.Append($"<{product.PlateNumber} ({item.ProductVisitsCount})>");
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
                foreach (var item in data.StatisticsProducts.Where(x => x.StatisticsCategoryId == i).Select(x => x.Id).ToList())
                {
                    lrUserStatisticsProduct.Add(new LrUserStatisticsProduct { LrUserId = lrUser.Id, LrUser = lrUser, StatisticsProductId = item });
                }
            }
            data.LrUserStatisticsProducts.AddRange(lrUserStatisticsProduct);
            data.SaveChanges();
        }       
    }
}
