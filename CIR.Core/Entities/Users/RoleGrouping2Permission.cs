using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIR.Core.Entities.Users
{
    [Keyless]
    [Table("RoleGrouping2Permission")]
    public partial class RoleGrouping2Permission
    {
        public long RoleGroupingId { get; set; }

        public long PermissionEnumId { get; set; }
    }

}
