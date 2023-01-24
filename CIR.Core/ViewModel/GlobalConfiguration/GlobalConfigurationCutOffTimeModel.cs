namespace CIR.Core.ViewModel.GlobalConfiguration
{
    public partial class GlobalConfigurationCutOffTimeModel
    {
        public long Id { get; set; }

        public long CountryId { get; set; }
        public string CutOffTime { get; set; }

        public short CutOffDay { get; set; }

    }

}
