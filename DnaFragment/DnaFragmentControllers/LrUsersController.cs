namespace DnaFragment.DnaFragmentControllers
{ 
    using System.Linq;
    using System.Threading.Tasks;
    using DnaFragment.Data.Models;
    using DnaFragment.Infrastructure;
    using DnaFragment.Models.Users;
    using DnaFragment.Services.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    
    
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

        [Authorize]
        public IActionResult All()
        {
            bool isAdmin = User.IsAdmin();
            var userId = User.GetId();
            var users = usersService.AllUsersDb(userId,isAdmin);
            return View(users);
        }

        [Authorize]        
        public  IActionResult DeleteUser(string userId)
        {
            if(userId == null)
            {
                return BadRequest();
            }
            
            usersService.DeleteUsersDb(userId);

            //await signInManager.SignOutAsync();

            SignOut();

            return RedirectToAction("Exit", "LrUsers");

        }
          public IActionResult Exit()
        {
            this.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}
