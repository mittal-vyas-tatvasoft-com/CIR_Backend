using CIR.Core.Entities.GlobalConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.ViewModel.GlobalConfig
{
    public class GlobalConfigurationEmailsModel : GlobalConfigurationEmails
    {
    }
    public class GlobalConfigurationEmailsGetModel : GlobalConfigurationEmails
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
