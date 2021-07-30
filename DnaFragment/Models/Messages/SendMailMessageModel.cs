namespace DnaFragment.Models.Messages
{
    public class SendMailMessageModel
    {
        public string To { get; set; }

        public string Password { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}
