using EStore.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EStore.Application.IRepositories;

namespace EStore.Infrastructure.Repositories
{
     public  class EmailRepository:IEmailRepository
     {
        private readonly IConfiguration _configuration;
        public EmailRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void SendMailNotification(string toEmail, string subject, string body)
        {
            try
            {
                string smtpHost = _configuration["EmailSettings:Host"];
                int smtpPort = Convert.ToInt32(_configuration["EmailSettings:Port"]);
                bool enableSSL = Convert.ToBoolean(_configuration["EmailSettings:UseSSL"]);
                string fromEmail = _configuration["EmailSettings:EmailId"];
                string fromName = _configuration["EmailSettings:Name"];
                string smtpUser = _configuration["EmailSettings:EmailId"];
                string smtpPassword = GetPassword();

                SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort);
                smtpClient.EnableSsl = enableSSL;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(fromEmail, smtpPassword);

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(fromEmail);
                mailMessage.To.Add(toEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                smtpClient.Send(mailMessage);
            }
            catch (SmtpException ex)
            {             
                throw new Exception("Error sending email", ex);
            }

        }
        private string GetPassword()
        {
            return _configuration["EmailSettings:Password"];
        }
    }
}
