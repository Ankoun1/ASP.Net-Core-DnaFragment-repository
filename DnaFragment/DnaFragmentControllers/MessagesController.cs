namespace Dnafragment.CarShopControllers
{    
    using System.Linq;   
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using DnaFragment.Infrastructure;
    using DnaFragment.Models.Messages;      
    using DnaFragment.Services.Messages;

    public class MessagesController :Controller
    {    
        private readonly IMessagesService messageService;

        public MessagesController(IMessagesService messageService)
        {    
            
            this.messageService = messageService;
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
