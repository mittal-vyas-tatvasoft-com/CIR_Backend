namespace CIR.Core.Entities.Websites;

public partial class Portal2GlobalConfigurationCutOffTimes
{
    public long Id { get; set; }

    public long PortalId { get; set; }

    public long GlobalConfigurationCutOffTimeId { get; set; }

    public TimeSpan CutOffTimeOverride { get; set; }

    public short CutOffDayOverride { get; set; }

}
