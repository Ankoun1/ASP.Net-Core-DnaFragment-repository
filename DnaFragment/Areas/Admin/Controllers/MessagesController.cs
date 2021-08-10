namespace DnaFragment.Areas.Admin.Controllers
{
    using DnaFragment.Models.Answers;
    using DnaFragment.Models.Messages;
    using DnaFragment.Services.Answers;
    using DnaFragment.Services.Mail;
    using DnaFragment.Services.Messages;
    using Microsoft.AspNetCore.Mvc;

    public class MessagesController : AdminController
    {
        private readonly IMessagesService messageService;
        private readonly ISendMailService sendMail;
        private readonly IAnswersService answersService;
        public MessagesController(IMessagesService messageService, ISendMailService sendMail, IAnswersService answersService)
        {
            this.sendMail = sendMail;
            this.messageService = messageService;
            this.answersService = answersService;
        }

        public IActionResult AddAnswer(string questId) => View();
       

        [HttpPost]       
        public IActionResult AddAnswer(string questId, AddAnswerModel model)
        {
           
            answersService.AddAnswerDb(questId, model.Description);

            return Redirect("/Questions/All");
        }

        public IActionResult Add(string lrUserId) => View();
       

        [HttpPost]       
        public IActionResult Add(string lrUserId, AddMessagesModel model)
        {           

            messageService.AddMessageDb(lrUserId, model.Description);

            return Redirect("/Messages/All");
        }

      
        public IActionResult Delete(string answerId)
        {
            
            answersService.DeleteAnswerDb(answerId);

            return this.Redirect("/Questions/All");
        }

        public IActionResult SendMail(string userId)
        {
            var emailUser = sendMail.UserEmail(userId);
            return View(new SendMailMessageModel { To = emailUser, Body = "<h1>Hay! From: DNAFragment-Asp.Net-Core-project</h1>" });
        }

        [HttpPost]
        public IActionResult SendMail(string userId, SendMailMessageModel mailModel)
        {
            sendMail.SendEmailAsync(userId, mailModel.Subject, mailModel.Body).Wait();

            ViewBag.message = "sucsses";
            var emailUser = sendMail.UserEmail(userId);

            return View(new SendMailMessageModel { To = emailUser, Subject = mailModel.Subject, Body = mailModel.Body });
        }
        
        public IActionResult Sms()
        {
            sendMail.SmsMessanger();
            return Redirect("/LrUsers/All");
        }       
    }
}
