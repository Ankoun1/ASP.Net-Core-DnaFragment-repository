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

        public IActionResult ShippingDelivery(string userId)
        {
            var users = usersService.ShippingOrders(userId);

                return View(users);
        }

        public IActionResult SendingTheRequest(int bagId)
        {

            usersService.Received(bagId);
            return Redirect("/Admin/LrUsers/ShippingDelivery");
        }

    }
}
