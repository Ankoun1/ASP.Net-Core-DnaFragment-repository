namespace DnaFragment.Models.Questions
{
    using System.Collections.Generic;
    using DnaFragment.Models.Questions;

    public class QuestionQueryModel
    {
        public QuestionQueryModel(AddQuestionModel question, List<QuestionListingViewModel> questions)
        {
            Question = question;
            Questions = questions;
        }

        public AddQuestionModel Question { get; set; }

        public List<QuestionListingViewModel> Questions { get; set; } = new List<QuestionListingViewModel>();
    }
}
