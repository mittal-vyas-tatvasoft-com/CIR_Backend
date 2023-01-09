using CIR.Common.Data;
using CIR.Common.Helper;
using CIR.Common.MailTemplate;
using CIR.Common.SystemConfig;
using CIR.Core.Entities.User;
using CIR.Core.Interfaces;
using CIR.Core.ViewModel;
using RandomStringCreator;

namespace CIR.Data.Data
{
    public class LoginRepository : ILoginRepository
    {
        private readonly CIRDbContext _CIRDBContext;
        private readonly EmailGeneration _emailGeneration;
        public LoginRepository(CIRDbContext context, EmailGeneration emailGeneration)
        {
            _CIRDBContext = context ??
                throw new ArgumentNullException(nameof(context));
            _emailGeneration = emailGeneration;
        }
        /// <summary>
        /// This method used by login user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        User ILoginRepository.Login(LoginModel model)
        {
            return _CIRDBContext.Users.FirstOrDefault((u) => u.UserName == model.UserName && u.Password == model.Password);

        }

        /// <summary>
        /// This method used by forgot password
        /// </summary>
        /// <param name="forgotPasswordModel"></param>
        /// <returns>Success status if its valid else failure</returns>
        public string ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            var user = _CIRDBContext.Users.Where(c => c.UserName == forgotPasswordModel.UserName && c.Email == forgotPasswordModel.Email).FirstOrDefault();
            if (user != null)
            {
                string randomString = SystemConfig.randomString;
                string newPassword = new StringCreator(randomString).Get(8);

                _CIRDBContext.Users.Where(x => x.Id == user.Id).ToList().ForEach((a =>
                {
                    a.Password = newPassword;
                }
                ));
                _CIRDBContext.SaveChanges();

                //Send NewPassword in Mail
                string mailSubject = MailTemplate.ForgotPasswordSubject();
                string mailBody = MailTemplate.ForgotPasswordTemplate(user);
                _emailGeneration.SendMail(forgotPasswordModel.Email, mailSubject, mailBody);

                return "Success";
            }
            return "Failure";
        }

        /// <summary>
        /// This method used by reset password
        /// </summary>
        /// <param name="resetPasswordModel"></param>
        /// <returns>Success status if its valid else failure</returns>
        public string ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            var user = _CIRDBContext.Users.Where(c => c.Id == resetPasswordModel.Id).FirstOrDefault();
            if (user != null)
            {
                if (user.Password == resetPasswordModel.OldPassword)
                {
                    user.Id = resetPasswordModel.Id;
                    user.Password = resetPasswordModel.NewPassword;
                    _CIRDBContext.Users.Update(user);
                    _CIRDBContext.SaveChanges();
                    return "Success";
                }
            }
            return "Failure";
        }
    }
}
