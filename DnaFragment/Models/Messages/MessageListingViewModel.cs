namespace DnaFragment.Models.Messages
{
    using System;

    public class MessageListingViewModel
    {
        public string Id { get; init; }

        public DateTime CreatedOn { get; init; }

        public string Description { get; set; }

        public bool IsAdministrator { get; set; }

        public string Name { get; init; }

    }
}
