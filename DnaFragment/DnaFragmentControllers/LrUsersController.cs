namespace DnaFragment.DnaFragmentControllers
{ 
    using System.Linq;
    using DnaFragment.Models.Users;
    using DnaFragment.Services.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class LrUsersController : Controller
    {
        
        private readonly IUsersService usersService;
        public LrUsersController( IUsersService usersService)
        {                                
            this.usersService = usersService;
        }       
                     
        public IActionResult ForgotPassword()
        {
            return View();
        }
       
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordUserModel resetPassword)
        {
            usersService.SendmailForgotPassword(resetPassword.Email);
            ViewBag.message = "sucsses";
            return View();
        }
        
        public IActionResult ResetPassword()
        {
            return View();
        }   
        
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordUserModel userModel)
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

            return Redirect("/Identity/Account/Login");
        }       

        [Authorize(Roles = "Administrator")]
        public IActionResult All()
        {
            var users = usersService.AllUsersDb();
            return View(users);
        }

        public IActionResult Logout()
        {
            this.SignOut();

            return Redirect("/");
        }
    }
}
