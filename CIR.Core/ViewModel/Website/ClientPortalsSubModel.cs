namespace CIR.Core.ViewModel.Website
{
    public class ClientPortalsSubModel
    {
        public long ClientId { get; set; }
        public string ClientName { get; set; }
        public List<PortalSubModel> Portals { get; set; }
    }
}
