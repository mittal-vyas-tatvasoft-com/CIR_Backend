using CIR.Core.Entities;
using CIR.Core.ViewModel;

namespace CIR.Core.Interfaces
{
    public interface ILoginService
    {
        public User Login(LoginModel model);
        public string ForgotPassword(ForgotPasswordModel forgotPasswordModel);
    }
}
