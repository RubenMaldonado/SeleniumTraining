using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTraining.Tools
{
    public class EmailSender
    {
        public void SendEmail(string mailto = "ruben.maldonado.tena@gmail.com", 
                                string mailfrom = "titaniumsoltest@gmail.com", 
                                string subject = "subject", 
                                string body = "body",
                                string attachementFileName = "")
        {
            var to = new MailAddress(mailto);
            
            var from = new MailAddress(mailfrom);

            var mail = new MailMessage(from, to);

            var smtp = new SmtpClient();

            mail.Subject = subject;

            mail.Body = body;

            if ( attachementFileName != "" )
            {
                mail.Attachments.Add( new Attachment( attachementFileName ) );
            }
            smtp.Host = "smtp.gmail.com";

            smtp.Port = 587;

            smtp.Credentials = new NetworkCredential(
                "titaniumsoltest@gmail.com", 
                "titaniumtest");

            smtp.EnableSsl = true;
            
            smtp.Send(mail);
        }
    }
}
