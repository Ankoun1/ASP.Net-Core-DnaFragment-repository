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

    public class LrUsersController : Controller
    {
        private readonly ISendMailService sendMail;
        private readonly DnaFragmentDbContext data;

        public LrUsersController( DnaFragmentDbContext data, ISendMailService sendMail)
        {         
            this.data = data;
            this.sendMail = sendMail;
        }
        
       

        [Authorize]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordUserModel resetPassword)
        {
           /* string resetPasswordId;
            int i = 0;
            foreach (var userResetPasswordId in data.Users.AsQueryable())
            {
                if (userResetPasswordId.ResetPasswordId == null)
                {
                    resetPasswordId = i.ToString();
                    for (int j = 0; j < 4; j++)
                    {
                        resetPasswordId += (j + i).ToString();
                        for (int c = 0; c < 3; c++)
                        {
                            resetPasswordId += (c + i).ToString();
                        }
                    }
                    userResetPasswordId.ResetPasswordId = resetPasswordId;
                }
                i++;
            }
            data.SaveChanges();*/

            Random generator = new Random();
            int r = generator.Next(100000, 1000000);
            
            var user = data.Users.Where(x => x.Email == resetPassword.Email).Select(x => new {UserName = x.FullName,Id = x.Id,ResetPasswordId = x.ResetPasswordId }).FirstOrDefault();
            var userTrue = data.Users.Find(user.Id);
            userTrue.ResetPasswordId = r;
            data.SaveChanges();
            sendMail.SendEmailAsync(user.Id, "Reset Password from DnaFragment", $"Hello {user.UserName},your identification number is {r} Follow the link https://localhost:44350/LrUsers/ResetPassword to reset your password").Wait();
            ViewBag.message = "sucsses";
            return View();
        }

        [Authorize]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordUserModel userModel)
        {
            if (!this.data.Users.Any(u => u.ResetPasswordId == userModel.RessetPasswordId))
            {
                this.ModelState.AddModelError(nameof(userModel.RessetPasswordId), $"User with '{userModel.RessetPasswordId}' not exists.");
            }


            if (!ModelState.IsValid || userModel.Password != userModel.ConfirmPassword)
            {
                return View(userModel);
            }
            var userId = data.Users.Where(x => x.ResetPasswordId == userModel.RessetPasswordId).Select(x => x.Id).FirstOrDefault();
            var user = data.Users.Find(userId);
            user.PasswordHash = userModel.Password;
            data.SaveChanges();
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
