using CIR.Core.Entities;
using CIR.Core.ViewModel;

namespace CIR.Core.Interfaces
{
    public interface ILoginRepository
    {
        public User Login(LoginModel model);
    }
}
