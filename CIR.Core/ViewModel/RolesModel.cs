using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.ViewModel
{
	public class RolesModel
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public Boolean AllPermissions { get; set; }
		public DateTime CreatedOn { get; set; }
	}
}
