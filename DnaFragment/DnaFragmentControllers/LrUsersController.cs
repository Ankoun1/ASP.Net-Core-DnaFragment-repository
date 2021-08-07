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
        public IActionResult All()
        {
            bool isAdmin = User.IsAdmin();
            var userId = User.GetId();
            if(userId == null)
            {
                return Redirect(RedirectToLogin);
            }
            if (!usersService.UserIsRegister(userId))
            {
                return BadRequest();
            }
            var users = usersService.AllUsersDb(userId,isAdmin);
            return View(users);
        }
        
        [Authorize]        
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var thisId = User.GetId();
            if (thisId == null )
            {
                return Redirect(RedirectToLogin);
            }
            if(!usersService.UserIsRegister(userId))
            {
                return BadRequest();
            }
            
            usersService.DeleteUsersDb(userId);

            await signInManager.SignOutAsync();           

            return RedirectToAction("Index", "Home");   
        }
        
    }
}
