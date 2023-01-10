using System.ComponentModel.DataAnnotations.Schema;

namespace CIR.Core.Entities.Users
{
    [Table("RoleGrouping")]
    public partial class RoleGrouping
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
    }
}
