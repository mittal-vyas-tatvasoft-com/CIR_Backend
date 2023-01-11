using System.ComponentModel.DataAnnotations;

namespace CIR.Core.Entities.GlobalConfig
{
    public class GlobalConfigurationReasons
    {
        [Key]
        [Required]
        public long Id { get; set; }

        [Required]
        public short Type { get; set; }

        [Required]
        public Boolean Enabled { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
