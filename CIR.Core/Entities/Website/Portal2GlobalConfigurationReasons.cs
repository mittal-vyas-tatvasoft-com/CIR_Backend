namespace CIR.Core.Entities.Websites
{
	public partial class Portal2GlobalConfigurationReasons
	{
		public long Id { get; set; }

		public long PortalId { get; set; }

		public long GlobalConfigurationReasonId { get; set; }

		public bool? Enabled { get; set; }

		public string? ContentOverride { get; set; }

		public long? DestinationId { get; set; }
	}
}
