namespace DnaFragment.Models.Messages
{
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants.DefaultConstants;

    public class AddMessagesModel   
    {
        [Required]
        [StringLength(
            DefaultDescriptionMaxLength,
            MinimumLength = DefaultDescriptionMinLength,
            ErrorMessage = "The field Description must be a string with a minimum length of {2}.")]
        public string Description { get; set; }
    }
}
