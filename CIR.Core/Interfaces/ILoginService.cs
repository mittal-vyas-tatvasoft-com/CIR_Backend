using CIR.Core.Entities.User;
using CIR.Core.ViewModel;

namespace CIR.Core.Interfaces
{
    public interface ILoginService
    {
        public User Login(LoginModel model);
        public string ForgotPassword(ForgotPasswordModel forgotPasswordModel);
        public string ResetPassword(ResetPasswordModel resetPasswordModel);
    }
}
