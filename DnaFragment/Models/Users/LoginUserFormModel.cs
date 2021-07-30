namespace DnaFragment.Models.Users
{
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants.LrUserConst;
    using static Data.DataConstants.DefaultConstants;

    public class LoginUserFormModel
    {
        [Required]
        [StringLength(DefaultMaxLength, MinimumLength = DefaultMinLength)]
        public string Username { get; init; }

        [Required]
        [StringLength(DefaultMaxLength, MinimumLength = DefaultMinLength)]
        public string Password { get; init; }
    }
}
