namespace DnaFragment.Models.Users
{
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants.DefaultConstants;
    using static Data.DataConstants.LrUserConst;

    public class ResetPasswordUserModel
    {

        [Range(100000,CodeNumber)]  
        public int? RessetPasswordId { get; init; }

        [Required]
        [StringLength(DefaultMaxLength, MinimumLength = MinLengthPassword)]
        public string Password { get; init; }

        [Required]
        [StringLength(DefaultMaxLength, MinimumLength = MinLengthPassword)]
        public string ConfirmPassword { get; init; }
    }
}