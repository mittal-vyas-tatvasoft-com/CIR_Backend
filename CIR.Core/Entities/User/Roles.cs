using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Entities.User
{
    public partial class Roles
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool AllPermissions { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
