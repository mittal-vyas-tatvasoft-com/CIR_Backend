namespace CIR.Core.Entities.GlobalConfiguration
{
    public partial class GlobalConfigurationCutOffTime
    {
        public long Id { get; set; }

        public long CountryId { get; set; }

        public TimeSpan CutOffTime { get; set; }

        public short CutOffDay { get; set; }

    }

}
