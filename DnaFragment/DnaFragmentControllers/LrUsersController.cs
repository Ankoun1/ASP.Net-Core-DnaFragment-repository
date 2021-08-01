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

    public class LrUsersController : Controller
    {
        private readonly ISendMailService sendMail;
        private readonly DnaFragmentDbContext data;

        public LrUsersController( DnaFragmentDbContext data, ISendMailService sendMail)
        {         
            this.data = data;
            this.sendMail = sendMail;
        }
        
        public IActionResult Register() => View();

       
        [HttpPost]
        public IActionResult Register(RegisterUser lrUser)
        {
            var userId = this.User.GetId();

            var userIdAlreadyRegister = this.data
                .LrUsers
                .Any(d => d.Id == userId);

            if (userIdAlreadyRegister)
            {
                return BadRequest();
            }

            if (this.data.LrUsers.Any(u => u.Username == lrUser.Username))
            {
                this.ModelState.AddModelError(nameof(lrUser.Username), $"User with '{lrUser.Username}' username already exists.");
            }

            if (this.data.LrUsers.Any(u => u.Email == lrUser.Email))
            {
                this.ModelState.AddModelError(nameof(lrUser.Email), $"User with '{lrUser.Email}' e-mail already exists.");
            }
            

            if (!ModelState.IsValid || lrUser.Password != lrUser.ConfirmPassword)
            {            
                return View(lrUser);
            }         

            var user = new LrUser
            {
                Id = userId,
                Username = lrUser.Username,
                Password = lrUser.Password,
                Email = lrUser.Email,
                PhoneNumber = lrUser.PhoneNumber,
                IsMechanic = lrUser.UserType == TypeMechanic,               
            };


            if (((lrUser.Username == "Anko" && lrUser.Password == "1234567")
                || (lrUser.Username == "Niki" && lrUser.Password == "12345678")) && lrUser.UserPreoritet == TypeAdministrator)
            {
                user.IsAdministrator = true;         

            }
          
            data.LrUsers.Add(user);

            data.SaveChanges();

            string resetPasswordId;
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
            data.SaveChanges();
            return Redirect("/LrUsers/Login");
        }
        public IActionResult ForgotPasswordEmail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPasswordEmail(ForgotPasswordUserModel resetPassword)
        {
            var user = data.Users.Where(x => x.Email == resetPassword.Email).Select(x => new {UserName = x.UserName,Email = x.Email,Id = x.Id,ResetPasswordId = x.ResetPasswordId }).FirstOrDefault();
            
            sendMail.SendEmailAsync(user.Id, "Reset Password from DnaFragment", $"Hello {user.UserName},your identification number is {user.ResetPasswordId} Follow the link https://localhost:44350/LrUsers/ResetPassword to reset your password").Wait();
            return Redirect("/LrUsers/ResetPassword");
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

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

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(LoginUserFormModel model)
        {
            var userId = this.data
               .LrUsers
               .Where(u => u.Username == model.Username && u.Password == model.Password)
               .Select(u => u.Id)
               .FirstOrDefault();

            if (userId == null)
            {
                this.ModelState.AddModelError(nameof(userId), "Username and password combination is not valid.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

           // this.SignIn(userId);         
          
            return Redirect("/Categories/All");
        }

        [Authorize]
        public IActionResult All()
        {
            var questionsOld = data.Questions.Where(x => !x.StopAutomaticDelete && (DateTime.Now - x.CreatedOn).TotalDays > 30);
            var answersOld = data.Answers.Where(x => !x.StopAutomaticDelete && (DateTime.Now - x.CreatedOn).TotalDays > 30);
            var messagesOld = data.Messages.Where(x => !x.StopAutomaticDelete && (DateTime.Now - x.CreatedOn).TotalDays > 360);
           
            data.Questions.RemoveRange(questionsOld);
            data.Answers.RemoveRange(answersOld);
            data.Messages.RemoveRange(messagesOld);
            data.SaveChanges();

            var users = data.LrUsers.Where(x => !x.IsAdministrator).Select(x => new UserListingViewModel
            {
                Id = x.Id,
                Username = x.Username,
                Email = x.Email,
                IsAdministrator = x.IsAdministrator
            }).ToList();

            return View(users);
        }

        public IActionResult Logout()
        {
            this.SignOut();

            return Redirect("/");
        }
    }
}
