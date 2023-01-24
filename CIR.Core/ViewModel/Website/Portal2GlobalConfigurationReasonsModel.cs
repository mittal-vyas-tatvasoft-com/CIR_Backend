namespace CIR.Core.ViewModel.Websites
{
    public partial class Portal2GlobalConfigurationReasonsModel
    {
        public long Id { get; set; }
        public long PortalId { get; set; }
        public long GlobalConfigurationReasonId { get; set; }
        public bool? Enabled { get; set; }
        public string? ContentOverride { get; set; }
        public long? DestinationId { get; set; }

        //office
        public long OficeIdPK { get; set; }
        public string AddressLine1 { get; set; } = null!;
        public string TownCity { get; set; } = null!;
        public long CountryCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Enabledoffice { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public short AddressType { get; set; }

        //portals
        public long PortalIdPK { get; set; }
        public long ClientId { get; set; }
        public long CurrencyId { get; set; }
        public long CountryId { get; set; }
        public long CultureId { get; set; }
        public short IntegrationLevel { get; set; }
        public bool? ReturnItemsEnabled { get; set; }
        public bool CreateResponse { get; set; }
        public bool CountReturnIdentifier { get; set; }

        //global config reason 
        public long globalconfigId { get; set; }
        public short Type { get; set; }
        public bool globalconfidEnabled { get; set; }
        public string Content { get; set; }
    }
}
