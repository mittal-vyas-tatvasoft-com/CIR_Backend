using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIR.Core.Entities.Users
{
    [Keyless]
    [Table("RoleGrouping2Culture")]
    public partial class RoleGrouping2Culture
    {
        public long RoleGroupingId { get; set; }

        public long CultureLcid { get; set; }
    }

}
