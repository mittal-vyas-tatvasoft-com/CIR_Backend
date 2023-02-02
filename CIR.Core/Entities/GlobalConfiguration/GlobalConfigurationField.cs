namespace CIR.Core.Entities.GlobalConfiguration
{
	public class GlobalConfigurationField
	{
		public long Id { get; set; }
		public long FieldTypeId { get; set; }
		public bool Enabled { get; set; }
		public bool Required { get; set; }
	}
}
