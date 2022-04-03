using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
namespace API.Helpers
{
    public class EmailHelper : IEmailHelper
    {
        private readonly IOptions<EmailSettings> _config;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public EmailHelper(IOptions<EmailSettings> config, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _config = config;
        }
        public void SendEmail(string userEmail, string confirmationLink, string token, string subject, string emailTemplate)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_config.Value.FromEmailId);
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = BuildEmailTemplate(confirmationLink,subject,emailTemplate);

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

        [NonAction]
        private string BuildEmailTemplate(string confirmationLink, string subject, string templateName)
        {
            var strMessage = "";
            try
            {
                var strTemplateFilePath = _hostingEnvironment.ContentRootPath + "/EmailTemplates/" + templateName;
                var reader = new StreamReader(strTemplateFilePath);
                strMessage = reader.ReadToEnd();
                reader.Close();
            }
            catch (Exception)
            {
               
            }
            strMessage = strMessage.Replace("[[[Title]]]", subject);
            strMessage = strMessage.Replace("[[[message]]]", confirmationLink);
            return strMessage;
        }
 
    }
}