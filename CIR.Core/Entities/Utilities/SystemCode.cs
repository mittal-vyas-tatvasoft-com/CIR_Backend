using System.ComponentModel.DataAnnotations.Schema;

namespace CIR.Core.Entities.Utilities
{

    [Table("SystemCodes")]
    public partial class SystemCode
    {
        public long Id { get; set; }

        public string Code { get; set; } = null!;

        public bool Dynamic { get; set; }

        public short? Area { get; set; }

    }

}
