

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
        public void AddAnswerDb(string questId,string userId,string description)
        {
            var quest = data.QuestionUsers.Where(x => x.LrUserId == userId && x.QuestionId == questId).FirstOrDefault();

            var answer = new Answer
            {
                Description = description + ".",
                //UserId = quest.UserId
                QuestionId = quest.QuestionId
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
