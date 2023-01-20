using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
