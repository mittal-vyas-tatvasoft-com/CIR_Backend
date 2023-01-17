namespace CIR.Core.Entities.Website
{
    public class Portals
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public long CurrencyId { get; set; }
        public long CountryId { get; set; }
        public long CultureId { get; set; }
        public long GlobalConfigurationFontId { get; set; }
        public short IntegrationLevel { get; set; }
        public short ReturnPolicy { get; set; }
        public bool ReturnItemsEnabled { get; set; }
        public string Entity { get; set; }
        public string Account { get; set; }
        public bool CreateResponse { get; set; }
        public bool CountReturnIdentifier { get; set; }
    }
}
