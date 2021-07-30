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
   
    public class MessagesController :Controller
    {      
        private readonly DnaFragmentDbContext data;

        public MessagesController(DnaFragmentDbContext data)
        {         
            this.data = data;
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

        public IActionResult SendMail()
        {           
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult SendMail(SendMailMessageModel mailModel)
        {
            /*var to = mailModel.To;
            var password = mailModel.Password;
            var subject = mailModel.Subject;
            var body = mailModel.Body;
            MailMessage mail = new MailMessage();
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new MailAddress("klimentinnik@gmail.com");
            mail.Sender = new MailAddress("ankonikolchevpl@gmail.com");
            mail.IsBodyHtml = true;
            //mail.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            //smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential(to,"abc");
            //smtp.Send(mail);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;

            smtp.Timeout = 30000;
           
                smtp.Send(mail);
          
           
            ViewBag.message = "The Mail Hass Been Send To Success!";*/
            SendEmail("ankonikolchevpl@gmail.com", "test", "Hi it worked!!",
           "azxczxczc@gmail.com", "azxczxczc@gmail.com", "xzczxc", "smtp.gmail.com", 587);
            return View();
        }

        [Authorize]
        public IActionResult All()
        {
            string acho = "asdadadsa";
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


        public IActionResult Delete(string messageId)
        {
            var message = data.Messages.Find(messageId);
            

            this.data.Messages.Remove(message);
            this.data.SaveChanges();

            return this.Redirect("/Messages/All");
        }

        public static void SendEmail(string address, string subject,
                 string message, string email, string username, string password,
                 string smtp, int port)
        {
            var loginInfo = new NetworkCredential(username, password);
            var msg = new MailMessage();
            var smtpClient = new SmtpClient(smtp, port);

            msg.From = new MailAddress(email);
            msg.To.Add(new MailAddress(address));
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);
        }

    }
}
