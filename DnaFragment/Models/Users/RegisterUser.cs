namespace DnaFragment.Models.Users
{
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants.DefaultConstants;
    using static Data.DataConstants.LrUserConst;

    public class RegisterUser
    {
        [Required]
        [StringLength(DefaultMaxLength, MinimumLength = MinLengthUsername)]
        public string Username { get; init; }

        [Required]
        [EmailAddress]
        public string Email { get; init; }  
        
       
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        public string PhoneNumber { get; init; }

        [Required]
        [StringLength(DefaultMaxLength, MinimumLength = MinLengthPassword)]
        public string Password { get; init; }

        [Required]
        [StringLength(DefaultMaxLength, MinimumLength = MinLengthPassword)]
        public string ConfirmPassword { get; init; }
        
        public string UserType { get; init; } 

        public string UserPreoritet { get; init; } 
    }
}
