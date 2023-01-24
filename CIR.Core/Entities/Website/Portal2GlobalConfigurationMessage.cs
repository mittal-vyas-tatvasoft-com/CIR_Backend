using System.ComponentModel.DataAnnotations;

namespace CIR.Core.Entities.Website
{
    public partial class Portal2GlobalConfigurationMessage
    {
        [Key]
        public long Id { get; set; }
        public long PortalId { get; set; }
        public long GlobalConfigurationMessageId { get; set; }
        public string ValueOverride { get; set; } = null!;
    }
}
