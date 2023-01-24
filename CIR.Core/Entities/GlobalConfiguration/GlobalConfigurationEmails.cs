using System.ComponentModel.DataAnnotations;

namespace CIR.Core.Entities.GlobalConfiguration
{
    public partial class GlobalConfigurationEmails
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long FieldTypeId { get; set; }
        [Required]
        public long CultureId { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Subject { get; set; }
    }
}
