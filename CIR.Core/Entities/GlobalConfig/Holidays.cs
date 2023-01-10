using System.ComponentModel.DataAnnotations;

namespace CIR.Core.Entities.GlobalConfig
{
	public partial class Holidays
	{
		[Key]
		public long Id { get; set; }
		public long CountryId { get; set; }
		public DateTime Date { get; set; }
		public string Description { get; set; }
	}
}
