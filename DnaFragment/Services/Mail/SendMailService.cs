namespace DnaFragment.Services.Mail
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DnaFragment.Data;
    using Microsoft.Extensions.Configuration;
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using Twilio;
    using Twilio.Rest.Api.V2010.Account;

    public class SendMailService : ISendMailService
    {
        private IConfiguration configuration;
        private readonly DnaFragmentDbContext data;
        public SendMailService(IConfiguration configuration, DnaFragmentDbContext data)
        {
            this.configuration = configuration;
            this.data = data;
        }

        public async Task SendEmailAsync(string userId, string subject, string content)
        {
            var emailUser = UserEmail(userId);
            var apiKey = configuration["SendGridApiKey"];
            var apiUser = configuration["SendGridUser"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("ankonikolchevpl@gmail.com", apiUser);
            var to = new EmailAddress("ankonikolchevpl@gmail.com");
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);
        }

        public string UserEmail(string userId)
        => data.Users.Where(x => x.Id == userId).Select(x => x.Email).FirstOrDefault();

        public void SmsMessanger(string to, string body)
        {
            string accountSid = Environment.GetEnvironmentVariable("AccountSID");
            string authToken = Environment.GetEnvironmentVariable("AuthToken");

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: body,
                from: new Twilio.Types.PhoneNumber("+12515806588"),
                to: new Twilio.Types.PhoneNumber("+359877668490")
            );

            Console.WriteLine(message.Body);
        }
    }
}