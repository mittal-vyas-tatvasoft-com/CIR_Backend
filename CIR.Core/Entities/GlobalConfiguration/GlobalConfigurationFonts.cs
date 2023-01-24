using System.ComponentModel.DataAnnotations;

namespace CIR.Core.Entities.GlobalConfiguration
{
    public partial class GlobalConfigurationFonts
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string FontFamily { get; set; }
        [Required]
        public Boolean IsDefault { get; set; }
        [Required]
        public Boolean Enabled { get; set; }
    }
}
