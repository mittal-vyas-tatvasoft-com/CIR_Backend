namespace CIR.Core.Entities.GlobalConfig
{
    public partial class GlobalConfigurationCurrency
    {
        public long Id { get; set; }

        public long CountryId { get; set; }

        public long CurrencyId { get; set; }

        public bool Enabled { get; set; }
    }
}
