namespace DnaFragment.Services.Mail
{
    using System.Linq;
    using System.Threading.Tasks;
    using DnaFragment.Data;
    using Microsoft.Extensions.Configuration;
    using SendGrid;
    using SendGrid.Helpers.Mail;

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
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("ankonikolchevpl@gmail.com", "DNA demo");
            var to = new EmailAddress(emailUser);
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);
        }

        public string UserEmail(string userId)
        => data.LrUsers.Where(x => x.Id == userId).Select(x => x.Email).FirstOrDefault();
       
    }
}
