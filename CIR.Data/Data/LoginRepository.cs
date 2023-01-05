using CIR.Common.Data;
using CIR.Core.Entities;
using CIR.Core.Interfaces;
using CIR.Core.ViewModel;

namespace CIR.Data.Data
{
    public class LoginRepository : ILoginRepository
    {
        private readonly CIRDbContext _CIRDBContext;
        public LoginRepository(CIRDbContext context)
        {
            _CIRDBContext = context ??
                throw new ArgumentNullException(nameof(context));
        }

        User ILoginRepository.Login(LoginModel model)
        {
            return _CIRDBContext.Users.FirstOrDefault((u) => u.UserName == model.UserName && u.Password == model.Password);

        }
    }
}
