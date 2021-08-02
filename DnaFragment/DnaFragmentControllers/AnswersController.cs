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
    using DnaFragment.Services.Answers;
    using DnaFragment.Services.Administrators;

    public class AnswersController : Controller
    {                
        private readonly IAnswersService answersService;     
        private readonly IAdministratorService adminService;     

        public AnswersController(IAnswersService answersService, IAdministratorService adminService)
        {                 
            this.answersService = answersService;
            this.adminService = adminService;
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult AddAnswer(string questId)
        {
            if (!adminService.UserIsRegister(User.GetId(), User.IsAdmin()))
            {
                return Unauthorized();
            }
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult AddAnswer(string questId, AddAnswerModel model)
        {
            //string input = questId;
            var userId = User.GetId();
            if (!adminService.UserIsRegister(User.GetId(), User.IsAdmin()))
            {
                return Unauthorized();
            }

            answersService.AddAnswerDb(questId, userId, model.Description);

            return Redirect("/Questions/All");
        }


        [Authorize]
        public IActionResult Delete(string answerId)
        {
            if (!adminService.UserIsRegister(User.GetId(), User.IsAdmin()))
            {
                return Unauthorized();
            }

            answersService.DeleteAnswerDb(answerId);

            return this.Redirect("/Questions/All");
        }

    }
}
