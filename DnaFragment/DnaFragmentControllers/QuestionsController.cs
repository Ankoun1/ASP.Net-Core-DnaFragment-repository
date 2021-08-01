namespace DnaFragment.DnaFragmentControllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DnaFragment.Data;
    using DnaFragment.Data.Models;
    using DnaFragment.Models.Question;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using DnaFragment.Infrastructure;

    public class QuestionsController : Controller
    {
        private readonly DnaFragmentDbContext data;     

        public QuestionsController(DnaFragmentDbContext data)
        {            
            this.data = data;           
        }   

        public IActionResult Questions() => View();

        [Authorize]
        [HttpPost]
        public IActionResult Questions(AddQuestionModel questionModel)
        {
            var userId = this.User.GetId();

            if (data.LrUsers.Any(x => x.Id == userId && x.IsAdministrator))
            {
                return Unauthorized();
            }           

            if (!ModelState.IsValid)
            {
                return View(questionModel);
            }
           
            var question = new Question
            {
                Description = questionModel.Description + "?"
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
            //data.Questions.Add(question2);
            data.SaveChanges();

            return Redirect("/Questions/All");
        }

        [Authorize]
        public IActionResult All()
        {
            var userId = this.User.GetId();

            var questions = data.Questions.AsQueryable();
            var questionsId = data.QuestionUsers.Where(x => x.LrUserId == userId).Select(x => x.QuestionId).ToList();
            List<Question> questionsTrue = new List<Question>();
            if (data.LrUsers.Any(x => x.Id == userId && !x.IsAdministrator))
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
                    StopAtomaticDelete = quest.StopAutomaticDelite
                };


                questionModel.Name = GetUserName(quest.Id);

                if (data.LrUsers.Any(x => x.Id == userId && x.IsAdministrator))
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

                        if (data.LrUsers.Any(x => x.Id == userId && x.IsAdministrator))
                        {
                            questionModel.IsAdministrator = true;
                        }
                        questionModel.AnswerDescription = answer.Description;
                        questionModel.AnswerId = answer.Id;

                        answerModels.Add(questionModel);
                    }

                }

            }
            return View(answerModels.OrderByDescending(x => x.CreatedOn).ThenBy(x => x.Name).ToList());
        }

        private string GetUserName(string lrQuestId)
        {
            var lrUserId = data.QuestionUsers.Where(x => x.QuestionId == lrQuestId && !x.LrUser.IsAdministrator).Select(x => x.LrUserId).FirstOrDefault();
            var lrUsername = data.LrUsers.Where(x => x.Id == lrUserId).Select(x => x.Username).FirstOrDefault();
            return lrUsername;
        }

        public IActionResult Delete(string questId)
        {
            var quest = data.Questions.Find(questId);
            if (data.Answers.Any(x => x.QuestionId == questId))
            {
                var answers = data.Answers.Where(x => x.QuestionId == questId).ToList();
                this.data.Answers.RemoveRange(answers);
            }

            this.data.Questions.Remove(quest);
            this.data.SaveChanges();

            return this.Redirect("/Questions/All");
        }

        [Authorize]
        public IActionResult AutomaticDelete(string questId)
        {
            var quest = data.Questions.Find(questId);
            if (data.Answers.Any(x => x.QuestionId == questId))
            {
                foreach (var answer in data.Answers.Where(x => x.QuestionId == questId).AsQueryable())
                {
                    answer.StopAutomaticDelite = true;
                }                
            }

            quest.StopAutomaticDelite = true;
            this.data.SaveChanges();
            ViewBag.message = "sucsses";
            return this.Redirect("/Questions/All");
        }
      
    }
}
