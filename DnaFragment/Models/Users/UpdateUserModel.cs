namespace DnaFragment.Models.Users
{
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants.DefaultConstants;
    using static Data.DataConstants.LrUserConst;

    public class UpdateUserModel
    {  
        [Required]
        [Display(Name = "Full Name")]
        [StringLength(MaxLengthFullName, MinimumLength = MinLengthFullName)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; init; }


        [Display(Name = "Phone Number")]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        public string PhoneNumber { get; init; }

        [Required]
        [StringLength(MaxLengthPassword, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = MinLengthPassword)]
        [DataType(DataType.Password)]
        public string Password { get; init; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; init; }

    }
}
