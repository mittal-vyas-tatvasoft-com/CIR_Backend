using CIR.Core.Entities;
using CIR.Core.Interfaces;
using CIR.Core.ViewModel;

namespace CIR.Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        public LoginService(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        User ILoginService.Login(LoginModel value)
        {
            return _loginRepository.Login(value); ;
        }
    }
}
