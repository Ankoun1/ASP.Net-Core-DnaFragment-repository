namespace DnaFragment.Models.Messages
{
    using System.ComponentModel.DataAnnotations;
    using static DnaFragment.Data.DataConstants.LrUserConst;

    public class SmsFormModel
    {
        [Required]
        public string Body { get; init; }

        [Display(Name = "Phone Number")]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        public string To { get; init; }       
    }
}
