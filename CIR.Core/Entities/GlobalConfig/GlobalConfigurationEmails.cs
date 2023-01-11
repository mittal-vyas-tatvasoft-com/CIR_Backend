using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Entities.GlobalConfig
{
    public partial class GlobalConfigurationEmails
    {
        public long Id { get; set; }
        public long FieldTypeId { get; set; }
        public long CultureId { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
    }
}
