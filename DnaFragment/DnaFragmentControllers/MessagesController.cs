namespace Dnafragment.CarShopControllers
{
    using System;
    using System.Linq;
    using DnaFragment.Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using DnaFragment.Infrastructure;
    using DnaFragment.Models.Messages;
    using DnaFragment.Data.Models;
    using System.Net.Mail;
    using System.Net;
    using DnaFragment.Services.Mail;

    public class MessagesController :Controller
    {      
        private readonly DnaFragmentDbContext data;
        private readonly ISendMailService sendMail;

        public MessagesController(DnaFragmentDbContext data, ISendMailService sendMail)
        {         
            this.data = data;
            this.sendMail = sendMail;
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

            //var user = data.Users.Where(x => x.Id == userId).FirstOrDefault();
            // List<Message> messages = new List<Message>(); 

            var message = new Message
              {
                 Description = model.Description,
                 LrUserId = lrUserId,
                 UserId = lrUserId
              };           
                          
            data.Messages.Add(message);

            data.SaveChanges();

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

            /*DateTime nowTime = DateTime.Now;
            
           if (DateTime.Now.Second - nowTime.Second > 8)
            {
                ViewBag.message = null;
            }
            else
            {
                ViewBag.message = "sucsses";
            }*/
           
            ViewBag.message = "sucsses";
            var emailUser = sendMail.UserEmail(userId);
            
            return View(new SendMailMessageModel { To = emailUser,Subject = mailModel.Subject,Body = mailModel.Body });
        }

        [Authorize]
        public IActionResult All()
        {
          
          var  userId = User.GetId();
            var messages = data.Messages.AsQueryable();            

            if (data.Users.Any(x => x.Id == userId && !User.IsAdmin()))
            {
              messages =  messages.Where(x => x.UserId == userId);
            }
            var mId = messages.Select(x => x.UserId).FirstOrDefault();
            var messageModels = messages.Select(x => new MessageListingViewModel
            {
                Id = x.Id,
                Name = x.User.FullName,
                CreatedOn = x.CreatedOn,
                Description = x.Description

            }).ToList();

            foreach (var message in messageModels)
            {
                if (data.Users.Any(x => x.Id == userId && User.IsAdmin()))
                {
                    message.IsAdministrator = true;
                }
            }
           /* if(messageModels.Count == 0)
            {
                messageModels.Add(new MessageListingViewModel
                {
                    Id = User.Id,
                    CreatedOn = DateTime.UtcNow,
                    Description = "В момента нямате нови съобщения"

                });
            }*/
            return View(messageModels);
        }

        [Authorize]
        public IActionResult Delete(string messageId)
        {
            var message = data.Messages.Find(messageId);
            if(message == null)
            {
                return BadRequest();
            }
            

            this.data.Messages.Remove(message);
            this.data.SaveChanges();

            return Redirect("/Messages/All");
        }      

    }
}
