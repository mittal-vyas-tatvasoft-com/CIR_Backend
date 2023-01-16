using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Entities.GlobalConfiguration
{
    public partial class CountryCode
    {
        public long Id { get; set; }

        public string Code { get; set; } = null!;

        public string CountryName { get; set; } = null!;
    }
}
