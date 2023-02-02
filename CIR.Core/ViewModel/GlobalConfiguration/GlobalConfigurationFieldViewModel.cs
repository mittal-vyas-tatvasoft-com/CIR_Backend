namespace CIR.Core.ViewModel.GlobalConfiguration
{
	public class GlobalConfigurationFieldViewModel
	{
		public long Id { get; set; }
		public string FieldName { get; set; }
		public long FieldTypeId { get; set; }
		public bool Enabled { get; set; }
		public bool Required { get; set; }
	}
}
