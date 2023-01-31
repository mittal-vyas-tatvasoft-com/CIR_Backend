namespace CIR.Core.ViewModel.Website
{
    public class OfficeModel
    {
        public List<officeVm> OfficeList { get; set; } = new();
        public int Count { get; set; }

        public class officeVm
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public string CountryName { get; set; }
            public string Address { get; set; }
        }
    }
}
