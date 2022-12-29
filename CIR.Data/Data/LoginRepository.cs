using CIR.Common.Data;
using CIR.Core.Entities;
using CIR.Core.Interfaces;
using CIR.Core.ViewModel;
using System.Text;

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
       
        User ILoginRepository.Login(LoginModel model)
        {
            return _CIRDBContext.Users.FirstOrDefault((u) => u.UserName == model.UserName && u.Password == model.Password);

        }
        public string ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            var user = _CIRDBContext.Users.Where(c => c.UserName == forgotPasswordModel.UserName && c.Email == forgotPasswordModel.Email).FirstOrDefault();
            if (user != null)
            {              
                const string lower = "abcdefghijklmnopqrstuvwxyz";
                const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                const string number = "1234567890";
                const string special = "!@#$%^&*";
                int length = 8;
                var middle = length / 2;
                StringBuilder res = new StringBuilder();
                Random rnd = new Random();
                while (0 < length--)
                {
                    if (middle == length)
                    {
                        res.Append(number[rnd.Next(number.Length)]);
                    }
                    else if (middle - 1 == length)
                    {
                        res.Append(special[rnd.Next(special.Length)]);
                    }
                    else
                    {
                        if (length % 2 == 0)
                        {
                            res.Append(lower[rnd.Next(lower.Length)]);
                        }
                        else
                        {
                            res.Append(upper[rnd.Next(upper.Length)]);
                        }
                    }
                }
                string newPassword = res.ToString();

                _CIRDBContext.Users.Where(x => x.Id == user.Id).ToList().ForEach((a =>
                {
                    a.Password = newPassword;
                }));
                _CIRDBContext.SaveChanges();

                return newPassword;

            }            
            return "";
        }
    }
}
