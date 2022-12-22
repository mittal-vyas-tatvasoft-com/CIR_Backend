using CIR.Application.Entities;
using CIR.Application.Interfaces;
using CIR.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        public LoginService(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }
        public User Login(LoginModel value)
        {
            return _loginRepository.Login(value);
        }
    }
}
