
using System.Threading.Tasks;

namespace Identity.Repositories
{
    public interface IMessageSender
    {
        public Task SendEmailAsync(string toEmail, string subject, string message, bool isMessageHtml = false);
    }
}