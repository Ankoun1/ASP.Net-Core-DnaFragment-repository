

namespace DnaFragment.Services.Questions
{
    using System.Collections.Generic;
    using System.Linq;
    using DnaFragment.Data;
    using DnaFragment.Data.Models;
    using DnaFragment.Models.Question;

    public class QuestionsService : IQuestionsService
    {
        private readonly DnaFragmentDbContext data;
        public QuestionsService(DnaFragmentDbContext data)
        {        
            this.data = data;
        }
        public void AddQuestion(string userId,string description)
        {         
            var question = new Question
            {
                Description = description + "?"
            };
            var users = data.LrUsers.Where(x => x.IsAdministrator).Select(x => x.Id).AsQueryable();
            var questionUsers = new List<QuestionUser>
            {
                new QuestionUser
                {
                QuestionId = question.Id,
                LrUserId = userId,
                UserId = userId
                },
                new QuestionUser
                {
                QuestionId = question.Id,
                LrUserId = users.Skip(1).FirstOrDefault(),
                UserId = users.Skip(1).FirstOrDefault()
                },
                new QuestionUser
                {
                QuestionId = question.Id,
                LrUserId = users.FirstOrDefault(),
                UserId = users.FirstOrDefault()
                }
            };

            question.QuestionUsers.Add(questionUsers[0]);

            data.Questions.Add(question);
            data.QuestionUsers.AddRange(questionUsers);
            data.SaveChanges();
        }

        public List<QuestionListingViewModel> AllQuestions(string userId, bool isAdmin)
        {
            var questions = data.Questions.AsQueryable();
            var questionsId = data.QuestionUsers.Where(x => x.LrUserId == userId).Select(x => x.QuestionId).ToList();
            List<Question> questionsTrue = new List<Question>();
            if (data.Users.Any(x => x.Id == userId && !isAdmin))
            {
                //questions = questions.Where(x => x.Id == x.QuestionUsers.Where(y => y.UserId == User.Id).Select(y => y.QuestionId).ToList())
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
                //questions = questions.Where(x => x.Id == questionId);
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
                    StopAtomaticDelete = quest.StopAutomaticDelete
                };


                questionModel.Name = GetUserName(quest.Id);

                if (data.Users.Any(x => x.Id == userId && isAdmin))
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
                            StopAtomaticDelete = quest.StopAtomaticDelete
                        };


                        questionModel.Name = GetUserName(quest.QuestionId);

                        if (data.Users.Any(x => x.Id == userId && isAdmin))
                        {
                            questionModel.IsAdministrator = true;
                        }
                        questionModel.AnswerDescription = answer.Description;
                        questionModel.AnswerId = answer.Id;

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

        public string GetUserName(string lrQuestId)
        {
            var lrUserId = data.QuestionUsers.Where(x => x.QuestionId == lrQuestId && !x.LrUser.IsAdministrator).Select(x => x.LrUserId).FirstOrDefault();
            var lrUsername = data.LrUsers.Where(x => x.Id == lrUserId).Select(x => x.Username).FirstOrDefault();
            return lrUsername;
        }
    }
}
