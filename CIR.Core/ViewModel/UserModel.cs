using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.ViewModel
{
    public class UserModel
    {
        public long Id { get; set; }

        public string UserName { get; set; } = null!;

        public string? Email { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public long RoleId { get; set; }

        public bool Enabled { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? EmployeeId { get; set; }
    }
}
