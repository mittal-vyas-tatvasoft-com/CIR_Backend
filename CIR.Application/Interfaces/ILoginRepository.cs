using CIR.Application.Entities;
using CIR.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Application.Interfaces
{
    public interface ILoginRepository
    {
        public User Login(LoginModel model);
    }
}
