namespace DnaFragment.Services.Questions
{
    using System.Collections.Generic;
    using DnaFragment.Models.Questions;

    public interface IQuestionsService
    {
        void AddQuestion(string userId,string description);

        List<QuestionListingViewModel> AllQuestions(byte sort,string userId,bool isAdmin);
       

        void DeleteQuest(string questId);

        void AutomaticDeleteQuest(string questId);
       
    }
}
