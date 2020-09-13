using System.Threading.Tasks;

namespace BadMelon.Data.Services
{
    public interface IEmailService
    {
        public Task SendEmail(string to, string subject, string body, bool isHtmlBody = false);
    }
}