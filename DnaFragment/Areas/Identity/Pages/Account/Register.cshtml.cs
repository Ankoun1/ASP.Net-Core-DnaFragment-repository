namespace DnaFragment.Areas.Identity.Pages.Account
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using DnaFragment.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;
    using static Data.DataConstants.DefaultConstants;
    using static Data.DataConstants.LrUserConst;
    using DnaFragment.Data;
    using System.Collections.Generic;
    using System.Linq;
    using DnaFragment.Services.Users;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;        
        private readonly DnaFragmentDbContext data;        
        private readonly IUsersService usersService;        

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            DnaFragmentDbContext data,
            IUsersService usersService
            )
        {
           this.userManager = userManager;
           this.signInManager = signInManager;          
           this.data = data;          
           this.usersService = usersService;          
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }       

        public class InputModel
        {
            [Required]
            [EmailAddress]            
            public string Email { get; set; }

            [Required]
            [Display(Name = "Full Name")]
            [StringLength(MaxLengthFullName,MinimumLength = MinLengthFullName)]
            public string FullName { get; set; }

            [Display(Name = "Phone Number")]
            [StringLength(PhoneNumberMaxLength,MinimumLength = PhoneNumberMinLength)]
            public string PhoneNumber { get; set; }

            [Required]
            [StringLength(MaxLengthPassword, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = MinLengthPassword)]
            [DataType(DataType.Password)]          
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;           
        }
        
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
           
            if (ModelState.IsValid && !data.LrUsers.Any(x => x.Email == Input.Email && x.IsDanger))
            {   
                
                var user = new User { UserName = Input.Email, Email = Input.Email,FullName = Input.FullName,PhoneNumber = Input.PhoneNumber };
                bool success = false;
                IdentityResult result = null;

                var finish = await CorrectRegistration(Input.Email, user,success,result);
                user = finish.Item3; 
                if ((finish.Item2 == null && finish.Item1) || (finish.Item2.Succeeded && !finish.Item1))
                {
                    await this.signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
               
            }            
            return Page();
        }

        private async Task<(bool, IdentityResult,User)> CorrectRegistration(string email,User user,bool success,IdentityResult result)
        {
            if (!data.LrUsers.Any(x => x.Email == email))
            {
                var oldEmailUserId = data.LrUsersOldEmails.Where(x => x.Email == email).Select(x => x.LrUserId).FirstOrDefault(); 
                var oldUser = data.LrUsers.Where(x => x.Id == oldEmailUserId).FirstOrDefault();
                if (oldUser == null)
                {
                    var lrUser = new LrUser();
                    if (data.LrUsers.Any())
                    {
                        lrUser.Email = email;                                                
                    }
                    else
                    {
                        lrUser.Email = "unknown@city.com";                                              
                    }
                    data.LrUsers.Add(lrUser);
                    data.SaveChanges();

                    usersService.AddNewLrUserInfoDb(lrUser);

                    result = await this.userManager.CreateAsync(user, Input.Password);                
                }
                else
                {
                    var currentUser = data.Users.Where(x => x.Email == oldUser.Email).FirstOrDefault();
                    if (currentUser != null)
                    {
                        user = await usersService.CorrectUpdate(oldUser,user.FullName, oldUser.Email, user.PhoneNumber, Input.Password, currentUser);
                        success = true;
                    }
                   
                }
                
            }
            return (success, result, user);
        }
    }
}

