using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIR.Core.Entities.Users
{
    [Keyless]
    [Table("RoleGrouping2SubSite")]
    public partial class RoleGrouping2SubSite
    {
        public long RoleGroupingId { get; set; }

        public long SubSiteId { get; set; }
    }
}
