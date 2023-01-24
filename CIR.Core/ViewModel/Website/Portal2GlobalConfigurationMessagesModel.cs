namespace CIR.Core.ViewModel.Website
{
    public class Portal2GlobalConfigurationMessagesModel
    {
        public long Id { get; set; }
        public long PortalId { get; set; }
        public long GlobalConfigurationMessageId { get; set; }
        public string ValueOverride { get; set; } = null!;
    }
}
