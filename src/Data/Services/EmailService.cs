using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BadMelon.Data.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<IEmailService> _log;
        private readonly SmtpClient client;
        private readonly string FromEmailAddress;

        public EmailService(IConfiguration config, ILogger<IEmailService> logger)
        {
            _log = logger;
            client = ConfigureClient(config);
            if (client != null && !string.IsNullOrEmpty(config["EmailServer:FromAddress"]))
            {
                FromEmailAddress = config["EmailServer:FromAddress"];
            }
        }

        public async Task SendEmail(string to, string subject, string body, bool isHtmlBody = false)
        {
            if (client == null) //No or bad config
            {
                _log.LogInformation($"[{DateTime.Now}] - Email Log: {to},{subject}:: {body}");
                return;
            }

            MailMessage message = new MailMessage(FromEmailAddress, to, subject, body);
            message.IsBodyHtml = isHtmlBody;

            try
            {
                client.Send(message);
            }
            catch (Exception e)
            {
                _log.LogError(e.Message);
            }
        }

        private static SmtpClient ConfigureClient(IConfiguration config)
        {
            var smtpServerHost = config["EmailServer:Host"];
            if (string.IsNullOrEmpty(smtpServerHost)) return null; //No configuration available

            var isSmtpServerPort = int.TryParse(config["EmailServer:Port"], out int smtpServerPort);
            var isSmtpServerSSL = bool.TryParse(config["EmailServer:SSL"], out bool smtpServerSSL);

            var smtpClient = new SmtpClient(smtpServerHost);

            if (isSmtpServerPort)
                smtpClient.Port = smtpServerPort;

            if (isSmtpServerSSL)
                smtpClient.EnableSsl = smtpServerSSL;

            return smtpClient;
        }
    }
}