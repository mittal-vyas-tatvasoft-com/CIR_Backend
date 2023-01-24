using CIR.Common.EmailGeneration;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace CIR.Common.Helper
{
    public class EmailGeneration
    {
        private readonly EmailModel _emailModel;
        public EmailGeneration(IOptions<EmailModel> emailModel)
        {
            _emailModel = emailModel.Value;

        }

        public void SendMail(string email, string mailSubject, string bodyTemplate)
        {
            try
            {
                string fromEmail = _emailModel.FromEmail;
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(fromEmail);
                mailMessage.To.Add(email);

                mailMessage.Subject = mailSubject;
                mailMessage.Body = bodyTemplate;
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.High;

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.UseDefaultCredentials = false;

                NetworkCredential networkCredential = new NetworkCredential(fromEmail, _emailModel.Password);
                smtpClient.Credentials = networkCredential;

                smtpClient.EnableSsl = _emailModel.Enablessl;
                smtpClient.Port = _emailModel.port;
                smtpClient.Host = _emailModel.Host;
                smtpClient.Send(mailMessage);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
