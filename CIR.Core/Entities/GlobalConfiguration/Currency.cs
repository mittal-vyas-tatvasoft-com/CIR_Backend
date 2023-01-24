using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Entities.GlobalConfiguration
{
    public partial class Currency
    {
        public long Id { get; set; }

        public string CodeName { get; set; } = null!;

        public string? Symbol { get; set; }
    }
}
