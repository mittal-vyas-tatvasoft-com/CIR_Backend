using CIR.Application.Entities;
using CIR.Application.Interfaces;
using CIR.Application.ViewModel;
using CIR.Common.Data;

namespace CIR.Data.Data
{
    public class LoginRepository: ILoginRepository
    {
        private readonly CIRDbContext _CIRDBContext;
        public LoginRepository(CIRDbContext context)
        {
            _CIRDBContext = context ??
                throw new ArgumentNullException(nameof(context));
        }

        User ILoginRepository.Login(Application.ViewModel.LoginModel model)
        {
            return _CIRDBContext.Users.FirstOrDefault((u) => u.UserName == model.UserName && u.Password == model.Password);
            
        }
    }
}
