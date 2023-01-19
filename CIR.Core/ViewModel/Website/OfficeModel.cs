namespace CIR.Core.ViewModel.Website
{
	public class OfficeModel
	{
		public List<officevm> OfficeList { get; set; } = new();
		public int Count { get; set; }

		public class officevm
		{
			public long Id { get; set; }
			public string Name { get; set; }
			public string country { get; set; }
			public string Address { get; set; }
		}
	}
}
