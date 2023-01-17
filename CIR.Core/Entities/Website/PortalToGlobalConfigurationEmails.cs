using System.ComponentModel.DataAnnotations;

namespace CIR.Core.Entities.Website
{
    public partial class PortalToGlobalConfigurationEmails
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long PortalId { get; set; }
        [Required]
        public long GlobalConfigurationEmailId { get; set; }
        [Required]
        public string ContentOverride { get; set; }
        [Required]
        public string SubjectOverride { get; set; }
    }
}
