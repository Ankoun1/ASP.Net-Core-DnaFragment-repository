namespace DnaFragment.DnaFragmentControllers
{
    using System;
    using System.Threading.Tasks;
    using DnaFragment.Data.Models;
    using DnaFragment.Infrastructure;
    using DnaFragment.Models.Users;
    using DnaFragment.Services.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;  
    using static WebConstants;

    public class LrUsersController : Controller
    {
        
        private readonly IUsersService usersService;
        private readonly SignInManager<User> signInManager;
        public LrUsersController( IUsersService usersService, SignInManager<User> signInManager)
        {                                
            this.usersService = usersService;
            this.signInManager = signInManager;
        }       
                     
        public IActionResult ForgotPassword()
        {
            return View();
        }
      
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordUserModel resetPassword)
        {
            var user = usersService.ValidEmail(resetPassword.Email);           

            if (!ModelState.IsValid || user == null)
            {
                return View(resetPassword);
            }
            usersService.SendmailForgotPassword(resetPassword.Email);
            ViewBag.message = "sucsses";
            return View();
        }
        
        public IActionResult ResetPassword()
        {
            return View();
        }   
        
        [HttpPost]
        public  IActionResult ResetPassword(ResetPasswordUserModel userModel)
        {
            if (!usersService.CodCheck(userModel.RessetPasswordId))
            {
                this.ModelState.AddModelError(nameof(userModel.RessetPasswordId), $"User with '{userModel.RessetPasswordId}' not exists.");
            }

            if (!ModelState.IsValid || userModel.Password != userModel.ConfirmPassword)
            {
                return View(userModel);
            }

            usersService.ResetPasswordDb(userModel.RessetPasswordId, userModel.Password);

            return Redirect(RedirectToLogin);
        }       

        [Authorize]
        public IActionResult Profile()
        {
            bool isAdmin = User.IsAdmin();
            var userId = User.GetId();
            if(userId == null)
            {
                return Redirect(RedirectToLogin);
            }
            if (User.IsAdmin())
            {
                return BadRequest();
            }
            var user = usersService.UsersDb(userId);
            if(user == null)
            {
                return Redirect(RedirectToRegister);
            }
           
            return View(user);
        } 

        [Authorize]
        public IActionResult UpdateProfile()
        {           
            if (User.IsAdmin())
            {
                return BadRequest();
            }
           
            return View();
        }
        
        [Authorize]
        [HttpPost]
        public IActionResult UpdateProfile(string userId, UpdateUserModel user)
        {
            if (userId == null || !usersService.UserIsRegister(userId))
            {
                return Redirect(RedirectToLogin);
            }

            if (User.IsAdmin())
            {
                return BadRequest();
            }

            if (!ModelState.IsValid && user.Password != user.ConfirmPassword)
            {
                return View(user);
            }
            usersService.UpdateDb(userId,user.FullName,user.Email,user.PhoneNumber,user.Password);

            return Redirect(RedirectToLogin);
        }
        [Authorize]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (!usersService.UserIsRegister(userId))
            {
                return BadRequest();
            }

            usersService.DeleteUsersDb(userId,User.IsAdmin());

            if (!User.IsAdmin())
            {
                await signInManager.SignOutAsync();
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
