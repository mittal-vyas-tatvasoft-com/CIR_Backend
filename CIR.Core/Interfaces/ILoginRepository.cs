using CIR.Core.Entities.User;
using CIR.Core.ViewModel;

namespace CIR.Core.Interfaces
{
    public interface ILoginRepository
    {
        public User Login(LoginModel model);
        public string ForgotPassword(ForgotPasswordModel forgotPasswordModel);
        public string ResetPassword(ResetPasswordModel resetPasswordModel);
    }
}
