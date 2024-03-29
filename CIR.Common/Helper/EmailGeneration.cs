﻿using CIR.Common.EmailGeneration;
using CIR.Core.Entities.Users;
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
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail)
                };
                mailMessage.To.Add(email);

                mailMessage.Subject = mailSubject;
                mailMessage.Body = bodyTemplate;
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.High;

                SmtpClient smtpClient = new SmtpClient
                {
                    UseDefaultCredentials = false
                };

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

        public static string ForgotPasswordTemplate(User user)
        {
            string template = "<p style=\"font-family:verdana;font-size:15px;\">" +
                                "Hello " + user.UserName + ", <br/><br/> " +
                                "Your New Login Password is : <b>" + user.Password + "</b>" +
                              "</p>";
            return template;
        }
        public static string ForgotPasswordSubject()
        {
            return "Forgot Password";
        }
    }
}
