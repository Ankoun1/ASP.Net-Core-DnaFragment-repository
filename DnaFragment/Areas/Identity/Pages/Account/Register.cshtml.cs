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
    using static Data.DataConstants.UserConst;
    using DnaFragment.Data;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;        
        private readonly DnaFragmentDbContext data;        

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            DnaFragmentDbContext data
            )
        {
           this.userManager = userManager;
           this.signInManager = signInManager;          
           this.data = data;          
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
           
            if (ModelState.IsValid)
            {
                var lrUser = new LrUser { Email = Input.Email };
                data.LrUsers.Add(lrUser);
                data.SaveChanges();

                var user = new User { UserName = Input.Email, Email = Input.Email,FullName = Input.FullName,PhoneNumber = Input.PhoneNumber };
                
                var result = await this.userManager.CreateAsync(user, Input.Password);
                
                if (result.Succeeded)
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
    }
}
