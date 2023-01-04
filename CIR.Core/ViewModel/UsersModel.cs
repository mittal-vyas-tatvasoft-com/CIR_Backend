using CIR.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.ViewModel
{
    public class UsersModel
    {
        public List<User> UsersList { get; set; } = new();

        public int Count { get; set; }
    }
}
