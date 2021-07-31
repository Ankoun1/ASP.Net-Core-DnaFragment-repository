

namespace DnaFragment.Services.Mail
{
    using System.Threading.Tasks;

    public interface ISendMailService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);


    }
}
