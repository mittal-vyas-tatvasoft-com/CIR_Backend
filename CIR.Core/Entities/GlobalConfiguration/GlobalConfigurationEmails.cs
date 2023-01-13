using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Entities.GlobalConfig
{
    public partial class GlobalConfigurationEmails
    {
        [Key]
        [Required]
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
