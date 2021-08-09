

namespace DnaFragment.Services.Questions
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using DnaFragment.Data;
    using DnaFragment.Data.Models;
    using DnaFragment.Models.Questions;   

    public class QuestionsService : IQuestionsService
    {
        private readonly DnaFragmentDbContext data;
        private readonly IMapper mapper;       

        public QuestionsService(DnaFragmentDbContext data, IMapper mapper)
        {        
            this.data = data;
            this.mapper = mapper;            
        }

        public void AddQuestion(string userId,string description)
        {
            if (data.Users.Any(x => x.Id == userId))
            {
                var question = new Question
                {
                    Description = description + "?",
                    UserId = userId
                };
                data.Questions.Add(question);
                data.SaveChanges();               
            }
         
        }

        public List<QuestionListingViewModel> AllQuestions(string userId,bool isAdmin)
        {
            var questions = data.Questions.AsQueryable();
            var questionsId = data.Questions.Where(x => x.UserId == userId).Select(x => x.Id).ToList(); ///
            List<Question> questionsTrue = new List<Question>();
            if (data.Users.Any(x => x.Id == userId) && !isAdmin)
            {                
                foreach (var quest in questions)
                {
                    foreach (var qustId in questionsId)
                    {
                        if (quest.Id == qustId)
                        {
                            questionsTrue.Add(quest);
                        }
                    }
                }               
            }
            else
            {
                questionsTrue = questions.ToList();
            }

            var answerModels = new List<QuestionListingViewModel>();
            string lrQuestId = null;
            string lrUsername = GetUserName(lrQuestId);

            foreach (var quest in questionsTrue.ToList())
            {
                var questionModel = new QuestionListingViewModel
                {
                    //Id = quest.Id,
                    CreatedOn = quest.CreatedOn,
                    Description = quest.Description,
                    QuestionId = quest.Id,
                    StopAtomaticDelete = quest.StopAutomaticDelete,                    
                    UserId = quest.UserId
                };

                questionModel.Name = GetUserName(quest.Id);

                if (data.Users.Any(x => x.Id == userId) && isAdmin)
                {
                    questionModel.IsAdministrator = true;
                }
                answerModels.Add(questionModel);
            }

            foreach (var quest in answerModels.ToList())
            {
                foreach (var answer in data.Answers.ToList())
                {
                    if (answer.QuestionId == quest.QuestionId)
                    {
                        answerModels.Remove(quest);
                        var questionModel = new QuestionListingViewModel
                        {
                            //Id = quest.Id,
                            CreatedOn = quest.CreatedOn,
                            Description = quest.Description,
                            QuestionId = quest.QuestionId,
                            StopAtomaticDelete = quest.StopAtomaticDelete,
                            AnswerId = answer.Id,
                            AnswerDescription = answer.Description,
                            UserId = quest.UserId
                        };

                        questionModel.Name = GetUserName(quest.QuestionId);

                        if (data.Users.Any(x => x.Id == userId) && isAdmin)
                        {
                            questionModel.IsAdministrator = true;
                        }                    

                        answerModels.Add(questionModel);
                    }

                }

            }           
            return (answerModels.OrderByDescending(x => x.CreatedOn).ThenBy(x => x.Name).ToList());
        }

        public void AutomaticDeleteQuest(string questId)
        {
            var quest = data.Questions.Find(questId);
            if (data.Answers.Any(x => x.QuestionId == questId))
            {
                foreach (var answer in data.Answers.Where(x => x.QuestionId == questId).AsQueryable())
                {
                    answer.StopAutomaticDelete = true;
                }
            }

            quest.StopAutomaticDelete = true;
            this.data.SaveChanges();
        }

        public void DeleteQuest(string questId)
        {
            var quest = data.Questions.Find(questId);
            if (data.Answers.Any(x => x.QuestionId == questId))
            {
                var answers = data.Answers.Where(x => x.QuestionId == questId).ToList();
                this.data.Answers.RemoveRange(answers);
            }

            this.data.Questions.Remove(quest);
            this.data.SaveChanges();
        }

        private string GetUserName(string lrQuestId)
        {            
            var lrUsername = data.Users.Where(x => x.Questions.Where(y => y.Id == lrQuestId)
                             .Select(y => y.UserId).FirstOrDefault() == x.Id).Select(x => x.FullName).FirstOrDefault();

            return lrUsername;
        }
    }
}
