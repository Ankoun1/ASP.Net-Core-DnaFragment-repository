namespace DnaFragment.DnaFragmentControllers
{
    using System;
    using System.Linq;
    using DnaFragment.Data;
    using DnaFragment.Data.Models;
    using DnaFragment.Models.Answers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using DnaFragment.Infrastructure;

    public class AnswersController : Controller
    {       
        private readonly DnaFragmentDbContext data;     

        public AnswersController(DnaFragmentDbContext data)
        {       
            this.data = data;            
        }

        [Authorize]
        public IActionResult AddAnswer(string questId)
        {
            if (!data.LrUsers.Any(x => x.Id == User.GetId() && x.IsAdministrator))
            {
                return Unauthorized();
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddAnswer(string questId, AddAnswerModel model)
        {
            //string input = questId;
            var userId = User.GetId();
            if (!data.LrUsers.Any(x => x.Id == userId && x.IsAdministrator))
            {
                return Unauthorized();
            }

            var quest = data.QuestionUsers.Where(x => x.LrUserId == userId && x.QuestionId == questId).FirstOrDefault();

            var answer = new Answer
            {            
                Description = model.Description + ".",
                //UserId = quest.UserId
                QuestionId = quest.QuestionId
            };

            data.Answers.Add(answer);

            data.SaveChanges();

            return Redirect("/Questions/All");
        }


        [Authorize]
        public IActionResult Delete(string answerId)
        {
            if (!data.LrUsers.Any(x => x.Id == User.GetId() && x.IsAdministrator))
            {
                return Unauthorized();
            }

            var answers = data.Answers.Where(x => x.Id == answerId);

            this.data.Answers.RemoveRange(answers);
            this.data.SaveChanges();

            return this.Redirect("/Questions/All");
        }

    }
}
