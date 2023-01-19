using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.ViewModel.Website
{
	public class PortalToGlobalConfigurationMessagesModel
	{
		public long Id { get; set; }
		public long PortalId { get; set; }
		public long GlobalConfigurationMessageId { get; set; }
		public string ValueOverride { get; set; } = null!;
	}
}
