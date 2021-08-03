namespace Dnafragment.CarShopControllers
{    
    using System.Linq;   
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using DnaFragment.Infrastructure;
    using DnaFragment.Models.Messages;   
    using DnaFragment.Services.Mail;
    using DnaFragment.Services.Messages;

    public class MessagesController :Controller
    {      
     
        private readonly ISendMailService sendMail;
        private readonly IMessagesService messageService;

        public MessagesController(ISendMailService sendMail, IMessagesService messageService)
        {    
            this.sendMail = sendMail;
            this.messageService = messageService;
        }


        [Authorize(Roles = "Administrator")]
        public IActionResult Add(string lrUserId)
        {
            if (!User.IsAdmin())
            {
                return Unauthorized();
            }

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Add(string lrUserId, AddMessagesModel model)
        {           
            if (!User.IsAdmin())
            {
                return Unauthorized();
            }

            messageService.AddMessageDb(lrUserId, model.Description);

            return Redirect("/Messages/All");
        }


        [Authorize(Roles = "Administrator")]
        public IActionResult SendMail(string userId)
        {
            var emailUser = sendMail.UserEmail(userId);
            return View(new SendMailMessageModel { To = emailUser,Body = "<h1>Hay! From: DNAFragment-Asp.Net-Core-project</h1>" });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult SendMail(string userId,SendMailMessageModel mailModel)        
        {         
            sendMail.SendEmailAsync(userId, mailModel.Subject,mailModel.Body).Wait();
           
            ViewBag.message = "sucsses";
            var emailUser = sendMail.UserEmail(userId);
            
            return View(new SendMailMessageModel { To = emailUser,Subject = mailModel.Subject,Body = mailModel.Body });
        }

        [Authorize]
        public IActionResult All()
        {          
          var  userId = User.GetId();
          bool  isAdmin = User.IsAdmin();

          var messageModels = messageService.AllMessageDb(userId, isAdmin);

          return View(messageModels);
        }

        [Authorize]
        public IActionResult Delete(string messageId)
        {            
            messageService.DeleteMessageDb(messageId);

            return Redirect("/Messages/All");
        }      

    }
}
