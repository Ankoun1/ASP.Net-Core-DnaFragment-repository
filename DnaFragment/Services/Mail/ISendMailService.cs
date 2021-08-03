namespace DnaFragment.Services.Mail
{
    using System.Threading.Tasks;

    public interface ISendMailService
    {
        string UserEmail(string userId);

        Task SendEmailAsync(string userId, string subject, string content);
    }
}
