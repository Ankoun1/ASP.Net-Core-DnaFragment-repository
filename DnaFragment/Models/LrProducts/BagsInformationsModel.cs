namespace DnaFragment.Models.LrProducts
{
    using System.ComponentModel.DataAnnotations;
    using static DnaFragment.Data.DataConstants.LrUserConst;
    public class BagsInformationsModel
    {
        [RegularExpression(@"[\w\s]{5,}[\d]+", ErrorMessage = "Address is not valid!")]
        public string Address { get; init; }

        [RegularExpression(@"^[\w\s]{3,}$", ErrorMessage = "City is not valid!")]
        public string City { get; set; }

        [Display(Name = "Phone Number")]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        public string PhoneNumber { get; set; }

    }
}
