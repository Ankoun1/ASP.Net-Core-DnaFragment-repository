namespace DnaFragment.Areas.Admin.Controllers
{
    using DnaFragment.Services.Users;
    using Microsoft.AspNetCore.Mvc;

    public class LrUsersController : AdminController
    {
       
        private readonly IUsersService usersService;
        
        public LrUsersController(IUsersService usersService)
        {
            this.usersService = usersService;            
        }
        public IActionResult All(byte sort)
        {
           
            var users =  usersService.UsersStatistics(sort);

            return View(users);
        } 
        public IActionResult Index()
        {           

            return View();
        }     
    }
}
