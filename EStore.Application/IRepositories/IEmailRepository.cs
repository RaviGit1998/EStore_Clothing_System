using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Application.IRepositories
{
    public interface IEmailRepository
    {
       void SendMailNotification(string toEmail, string subject, string body);
    }
}
