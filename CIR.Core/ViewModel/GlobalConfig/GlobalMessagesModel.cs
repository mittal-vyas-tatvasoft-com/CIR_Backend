using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.ViewModel.GlobalConfig
{
	public class GlobalMessagesModel
	{
		public long Id { get; set; }

		public Int16 Type { get; set; }

		public string Content { get; set; }

		public long CultureID { get; set; }
	}
}
