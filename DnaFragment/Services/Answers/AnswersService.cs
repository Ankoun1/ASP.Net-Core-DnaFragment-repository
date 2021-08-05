

namespace DnaFragment.Services.Answers
{
    using System.Linq;
    using DnaFragment.Data;
    using DnaFragment.Data.Models;

    public class AnswersService : IAnswersService
    {
        private readonly DnaFragmentDbContext data;
        public AnswersService(DnaFragmentDbContext data)
        {            
            this.data = data;
        }
        public void AddAnswerDb(string questId,string description)
        {
            var quest = data.Questions.Where(x =>  x.Id == questId).FirstOrDefault(); ///

            var answer = new Answer
            {
                Description = description + ".",               
                QuestionId = quest.Id
            };

            data.Answers.Add(answer);

            data.SaveChanges();
        }

        public void DeleteAnswerDb(string answerId)
        {
            var answers = data.Answers.Where(x => x.Id == answerId);

            this.data.Answers.RemoveRange(answers);
            this.data.SaveChanges();
        }
    }
}
