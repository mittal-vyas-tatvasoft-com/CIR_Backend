namespace CIR.Core.ViewModel.GlobalConfiguration
{
    public class GlobalConfigurationWeekendsModel
    {
        public List<WeekendModel> WeekendsList { get; set; } = new();

        public int Count { get; set; }

    }
    public class WeekendModel
    {
        public long Id { get; set; }
        public long CountryId { get; set; }

        public long DayOfWeekId
        {
            get; set;
        }
        public string CountryName { get; set; }

        public string CountryCode { get; set; }

        public string DayOfWeek { get; set; }
        public int TotalCount { get; set; }

    }
}
