namespace CIR.Core.Entities
{
    public partial class SubSite
    {
        public long Id { get; set; }

        public string? Directory { get; set; }

        public string DisplayName { get; set; } = null!;

        public string? Domain { get; set; }

        public string? Description { get; set; }

        public long? AssetId { get; set; }

        public bool? Enabled { get; set; }

        public string? SystemEmailFromAddress { get; set; }

        public long? FaviconAssetId { get; set; }

        public string? RobotTxt { get; set; }

        public bool Stopped { get; set; }

        public bool? ShowTax { get; set; }

        public long? PortalId { get; set; }

        public string? BccemailAddress { get; set; }

        public string? CloudFrontDistributionId { get; set; }

        public bool EmailStopped { get; set; }
    }
}
