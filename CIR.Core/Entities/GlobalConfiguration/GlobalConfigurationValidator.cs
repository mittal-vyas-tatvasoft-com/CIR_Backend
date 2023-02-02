namespace CIR.Core.Entities.GlobalConfiguration
{
	public class GlobalConfigurationValidator
	{
		public long Id { get; set; }
		public long FieldTypeId { get; set; }
		public long CultureId { get; set; }
		public bool Enabled { get; set; }
		public string Regex { get; set; } = null!;
		public string ErrorMessage { get; set; } = null!;
	}
}
