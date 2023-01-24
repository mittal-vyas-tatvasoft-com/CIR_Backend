namespace CIR.Core.ViewModel.Website
{
    public partial class Portal2GlobalConfigurationCutOffTimesModel
    {
        public long Id { get; set; }

        public long PortalId { get; set; }

        public long GlobalConfigurationCutOffTimeId { get; set; }
        public string CutOffTimeOverride { get; set; }

        public short CutOffDayOverride { get; set; }

    }

}
