using CIR.Core.Entities.User;

namespace CIR.Common.MailTemplate
{
    public static class MailTemplate
    {
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
