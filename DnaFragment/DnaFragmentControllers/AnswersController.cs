namespace DnaFragment.DnaFragmentControllers
{    
    using System.Linq;    
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

        [Authorize]
        public IActionResult Delete(string answerId)
        {
            if (!adminService.UserIsRegister(User.GetId()))
            {
                return Unauthorized();
            }

            answersService.DeleteAnswerDb(answerId);

            return this.Redirect("/Questions/All");
        }

    }
}
