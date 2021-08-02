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
    using DnaFragment.Areas.Identity.Pages.Account;
    using DnaFragment.Services.Questions;

    public class QuestionsController : Controller
    {
          
        private readonly IQuestionsService questionsService;     

        public QuestionsController(IQuestionsService questionsService)
        {                     
            this.questionsService = questionsService;
        }
        
        [Authorize]
        public IActionResult Questions() => View();

        [Authorize]
        [HttpPost]
        public IActionResult Questions(AddQuestionModel questionModel)
        {
            var userId = this.User.GetId();           

            if (data.Users.Any(x => x.Id == userId && User.IsAdmin()))
            {
                return Unauthorized();
            }            

            if (!ModelState.IsValid)
            {
                return View(questionModel);
            }

            questionsService.AddQuestion(userId, questionModel.Description);

            return Redirect("/Questions/All");
        }

        [Authorize]
        public IActionResult All()
        {
            var userId = User.GetId();
            bool isAdmin = User.IsAdmin();

            var questions = questionsService.AllQuestions(userId, isAdmin);

            return View(questions);
        }       

        [Authorize]
        public IActionResult Delete(string questId)
        {
            questionsService.DeleteQuest(questId);

            return this.Redirect("/Questions/All");
        }

        [Authorize]
        public IActionResult AutomaticDelete(string questId)
        {
            questionsService.AutomaticDeleteQuest(questId);
            ViewBag.message = "sucsses";
            return this.Redirect("/Questions/All");
        }      
    }
}
