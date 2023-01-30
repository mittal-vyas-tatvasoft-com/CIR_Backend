namespace CIR.Core.Entities
{
	public class Portal2GlobalConfigurationStyle
	{
		public long Id { get; set; }

		public long PortalId { get; set; }

		public long GlobalConfigurationStyleId { get; set; }

		public string ValueOverride { get; set; } = null!;
	}
}
