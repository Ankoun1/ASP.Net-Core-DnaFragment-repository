using System.Collections.Generic;
using DnaFragment.Models.Messages;

namespace DnaFragment.Services.Messages
{
    public interface IMessagesService
    {
        void AddMessageDb(string userId,string description);

        List<MessageListingViewModel> AllMessageDb(string userId,bool isAdmin);

        void DeleteMessageDb(string messageId);
    }
}
