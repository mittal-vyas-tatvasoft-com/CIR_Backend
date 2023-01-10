using System.ComponentModel.DataAnnotations;

namespace CIR.Core.Entities.GlobalConfig
{
	public partial class GlobalConfigurationFonts
	{
		[Key]
		public long Id { get; set; }
		public string Name { get; set; }
		public string FontFamily { get; set; }
		public Boolean IsDefault { get; set; }
		public Boolean Enabled { get; set; }
	}
}
