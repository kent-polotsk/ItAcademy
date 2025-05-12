using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNA.Services.Abstractions
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string toEmail, string code);
        public Task SendNewsletterAsync(string toEmail, string message = "");
    }
}
