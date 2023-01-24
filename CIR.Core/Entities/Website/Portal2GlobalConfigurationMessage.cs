using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Entities.Websites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
