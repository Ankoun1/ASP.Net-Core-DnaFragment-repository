namespace DnaFragment.Services.Messages
{
    using System.Collections.Generic;
    using System.Linq;
    using DnaFragment.Data;
    using DnaFragment.Data.Models;
    using DnaFragment.Models.Messages;

    public class MessagesService : IMessagesService
    {
        private readonly DnaFragmentDbContext data;

        public MessagesService(DnaFragmentDbContext data)
        {
            this.data = data;
        }
        public void AddMessageDb(string userId,string description)
        {
            var message = new Message
            {
                Description = description,                
                UserId = userId
            };

            data.Messages.Add(message);

            data.SaveChanges();
        }

        public List<MessageListingViewModel> AllMessageDb(string userId,bool isAdmin)
        {
            
            var messages = data.Messages.AsQueryable();

            if (data.Users.Any(x => x.Id == userId) && !isAdmin)
            {
                messages = messages.Where(x => x.UserId == userId);
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
                if (data.Users.Any(x => x.Id == userId) && isAdmin)
                {
                    message.IsAdministrator = true;
                }
            }

            return messageModels;
        }

        public void DeleteMessageDb(string messageId)
        {
            var message = data.Messages.Find(messageId);
            if (message == null)
            {
                return;
            }

            this.data.Messages.Remove(message);
            this.data.SaveChanges();
        }
    }
}
