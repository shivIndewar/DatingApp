using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace API.Helpers
{
    public class EmailHelper : IEmailHelper
    {
        private readonly IOptions<EmailSettings> _config;
        public EmailHelper(IOptions<EmailSettings> config)
        {
            _config = config;
        }
        public void SendEmail(string userEmail, string confirmationLink, string token, string subject)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_config.Value.FromEmailId);
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = confirmationLink;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(_config.Value.FromEmailId,_config.Value.FromEmailPassword);
            client.Host = "smtp.gmail.com";
            client.Port=587;
            client.EnableSsl = true;

            try
            {
               client.SendAsync(mailMessage, token);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}