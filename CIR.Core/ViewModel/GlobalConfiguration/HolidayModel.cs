namespace CIR.Core.ViewModel.GlobalConfiguration
{
    public class HolidayModel
    {
        public long Id { get; set; }
        public long CountryId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }
}
