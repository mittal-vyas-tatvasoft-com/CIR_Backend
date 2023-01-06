using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Entities.GlobalConfig
{
    public partial class GlobalConfigurationCurrency
    {
        public long Id { get; set; }

        public long CountryId { get; set; }

        public long CurrencyId { get; set; }

        public bool Enabled { get; set; }       

        public virtual CountryCode Country { get; set; } = null!;

        public virtual Currency Currency { get; set; } = null!;
        public List<Currency> Currencies { get; set; }
    }
}
