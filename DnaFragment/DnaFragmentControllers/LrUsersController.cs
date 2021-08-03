namespace DnaFragment.DnaFragmentControllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DnaFragment.Data;
    using DnaFragment.Data.Models;
    using DnaFragment.Models.Users;
    using Microsoft.AspNetCore.Authorization;
    using DnaFragment.Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using static Data.DataConstants.LrUserConst;
    using DnaFragment.Services.Mail;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.WebUtilities;
    using System.Text;
    using System.Threading.Tasks;

    public class LrUsersController : Controller
    {
        private readonly ISendMailService sendMail;
        private readonly DnaFragmentDbContext data;
        private readonly IPasswordHasher passwordHasher;
        private readonly UserManager<User> _userManager;
        public LrUsersController( DnaFragmentDbContext data, ISendMailService sendMail, IPasswordHasher passwordHasher,UserManager<User> _userManager)
        {         
            this.data = data;
            this.sendMail = sendMail;
            this.passwordHasher = passwordHasher;
            this._userManager = _userManager;
        }       
                     
        public IActionResult ForgotPassword()
        {
            return View();
        }
       
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordUserModel resetPassword)
        {           
            Random generator = new Random();
            int r = generator.Next(100000, 1000000);            
            
            var user = data.Users.Where(x => x.Email == resetPassword.Email).FirstOrDefault();           
            
            user.ResetPasswordId = r;
            data.SaveChanges();
            sendMail.SendEmailAsync(user.Id, "Reset Password from DnaFragment", $"Hello {user.UserName},your identification number is {r} Follow the link https://localhost:44350/LrUsers/ResetPassword to reset your password").Wait();
            ViewBag.message = "sucsses";
            return View();
        }
        
        public IActionResult ResetPassword()
        {
            return View();
        }   
        
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordUserModel userModel)
        {
            if (!this.data.Users.Any(u => u.ResetPasswordId == userModel.RessetPasswordId))
            {
                this.ModelState.AddModelError(nameof(userModel.RessetPasswordId), $"User with '{userModel.RessetPasswordId}' not exists.");
            }

            if (!ModelState.IsValid || userModel.Password != userModel.ConfirmPassword)
            {
                return View(userModel);
            }         
                        
            var user = data.Users.Where(x => x.ResetPasswordId == userModel.RessetPasswordId).FirstOrDefault();
            
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var  code1 = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code1));
            var userReset = await _userManager.FindByEmailAsync(user.Email);      
            var result = await _userManager.ResetPasswordAsync(userReset, Code, userModel.Password);
            
            return Redirect("/Identity/Account/Login");
        }       

        [Authorize(Roles = "Administrator")]
        public IActionResult All()
        {
            DateTime curentdate = DateTime.UtcNow;            
            var questionsOld = data.Questions.ToList().Where(x => !x.StopAutomaticDelete && (curentdate - x.CreatedOn).TotalDays > 30.0);
            var answersOld = data.Answers.ToList().Where(x => !x.StopAutomaticDelete && (curentdate - x.CreatedOn).TotalDays > 30);
            var messagesOld = data.Messages.ToList().Where(x => !x.StopAutomaticDelete && (curentdate - x.CreatedOn).TotalDays > 360);
           
            data.Questions.RemoveRange(questionsOld);
            data.Answers.RemoveRange(answersOld);
            data.Messages.RemoveRange(messagesOld);
            data.SaveChanges();

            var users = data.Users.Where(x => !x.IsAdministrator).Select(x => new UserListingViewModel
            {
                Id = x.Id,
                Username = x.FullName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,               
                IsAdministrator = x.IsAdministrator,
                
            }).ToList();
            foreach (var user in users)
            {
                user.NumberMessages = data.Messages.Where(x => x.UserId == user.Id).Count();
                user.NumberQuestions = data.QuestionUsers.Where(x => x.UserId == user.Id).Count();
            }
            return View(users);
        }

        public IActionResult Logout()
        {
            this.SignOut();

            return Redirect("/");
        }
    }
}
