using CIR.Core.Entities.GlobalConfiguration;

namespace CIR.Core.ViewModel.GlobalConfiguration
{
	public class HolidayModel
	{
		public List<Holidays> HolidayList { get; set; } = new();
		public int Count { get; set; }
	}
}
