namespace DnaFragment.Areas.Admin.Controllers
{
    using DnaFragment.Services.Mail;
    using DnaFragment.Services.Users;
    using Microsoft.AspNetCore.Mvc;

    public class LrUsersController : AdminController
    {
       
        private readonly IUsersService usersService;
        private readonly ISendMailService sendMail;

        public LrUsersController(IUsersService usersService, ISendMailService sendMail)
        {
            this.usersService = usersService;            
            this.sendMail = sendMail;            
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
            //sendMail.SmsMessanger("0876408508","Очквайте доставка до 1 ден и потвърдете с кода за идентификация на поръчката!DnaFragment♡");
            return Redirect("/Admin/LrUsers/ShippingDelivery");
        }

    }
}
