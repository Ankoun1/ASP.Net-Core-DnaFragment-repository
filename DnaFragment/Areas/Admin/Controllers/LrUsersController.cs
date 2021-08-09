namespace DnaFragment.Areas.Admin.Controllers
{
    using System.Threading.Tasks;
    using DnaFragment.Data.Models;
    using DnaFragment.Models.Messages;
    using DnaFragment.Services.LrProducts.Models;
    using DnaFragment.Services.Mail;
    using DnaFragment.Services.Messages;
    using DnaFragment.Services.Users;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class LrUsersController : AdminController
    {
       
        private readonly IUsersService usersService;
        
        public LrUsersController(IUsersService usersService)
        {
            this.usersService = usersService;            
        }
        public IActionResult All()
        {
            var users = usersService.UsersStatistics();

            return View(users);
        } 
        public IActionResult Index()
        {           

            return View();
        }
        

    }
}
