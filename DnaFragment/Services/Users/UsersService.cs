namespace DnaFragment.Services.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
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


        public async  void ResetPasswordDb(int? code, string password)
        {
            
            var user = data.Users.Where(x => x.ResetPasswordId == code).FirstOrDefault();
           
                var code1 = await _userManager.GeneratePasswordResetTokenAsync(user);
                var code2 = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code1));
                var Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code2));
                var userReset = await _userManager.FindByEmailAsync(user.Email);
                var result = await _userManager.ResetPasswordAsync(userReset, Code, password);
           
        }

        public List<UserListingViewModel> AllUsersDb(string userId,bool isAdmin)
        {
           
            var users = data.Users.AsQueryable();
            if (isAdmin)
            {
                AutomaticDeleteDb();

                users = users.Where(x => !x.IsAdministrator);
            }
            else
            {
                users = users.Where(x => x.Id == userId);
            }

            var userListing = users.Select(x => new UserListingViewModel
            {
                Id = x.Id,
                Username = x.FullName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                IsAdministrator = x.IsAdministrator,

            }).ToList();


            foreach (var user in userListing)
            {
                user.NumberMessages = data.Messages.Where(x => x.UserId == user.Id).Count();
                user.NumberQuestions = data.Questions.Where(x => x.UserId == user.Id).Count(); ///
            }
            return userListing;
        }

        public bool CodCheck(int? code)
        => this.data.Users.Any(u => u.ResetPasswordId == code);
    
        public void DeleteUsersDb(string userId)
        {
            if (UserIsRegister(userId)) 
            {               
                var user = data.Users.Find(userId);
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
    }
}
