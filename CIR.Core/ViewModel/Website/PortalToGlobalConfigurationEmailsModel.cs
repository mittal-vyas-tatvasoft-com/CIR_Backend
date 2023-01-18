using CIR.Core.Entities.Website;

namespace CIR.Core.ViewModel.Website
{
    public class PortalToGlobalConfigurationEmailsModel : PortalToGlobalConfigurationEmails
    {
    }
    public class PortalToGlobalConfigurationEmailsGetModel : PortalToGlobalConfigurationEmails
    {
        public bool Reference { get; set; }
        public bool BookingId { get; set; }
        public bool OrderNumber { get; set; }
        public bool CustomerEmail { get; set; }
        public bool CustomerName { get; set; }
        public bool CollectionDate { get; set; }
        public bool CollectionAddress { get; set; }
        public bool TrackingURL { get; set; }
        public bool LabelURL { get; set; }
        public bool BookingURL { get; set; }
    }
}
