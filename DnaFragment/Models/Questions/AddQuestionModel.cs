namespace DnaFragment.Models.Questions
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants.DefaultConstants;

    public class AddQuestionModel
    {
        [Required]
        [StringLength(
            DefaultDescriptionMaxLength,
            MinimumLength = DefaultDescriptionMinLength,
            ErrorMessage = "The field Description must be a string with a minimum length of {2}.")]
        public string Description { get; set; }

        public List<QuestionListingViewModel> Questions { get; set; } = new List<QuestionListingViewModel>();

    }
}
