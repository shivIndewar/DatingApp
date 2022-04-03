using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IEmailHelper
    {
        void SendEmail(string userEmail, string confirmationLink, string token, string subject, string templateName);
    }
}