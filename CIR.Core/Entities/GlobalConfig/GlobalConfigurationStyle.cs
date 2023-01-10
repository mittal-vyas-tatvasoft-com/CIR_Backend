using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Entities.GlobalConfig
{
    public partial class GlobalConfigurationStyle
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int TypeCode { get; set; }

        public string TypeName { get; set; } = null!;

        public string ValueType { get; set; } = null!;

        public string Value { get; set; } = null!;

        public double SortOrder { get; set; }

    }
}
