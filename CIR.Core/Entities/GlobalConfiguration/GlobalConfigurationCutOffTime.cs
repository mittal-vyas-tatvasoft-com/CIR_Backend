using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Entities.GlobalConfiguration
{
    public partial class GlobalConfigurationCutOffTime
    {
        public long Id { get; set; }

        public long CountryId { get; set; }

        public TimeSpan CutOffTime { get; set; }

        public short CutOffDay { get; set; }

    }

}
