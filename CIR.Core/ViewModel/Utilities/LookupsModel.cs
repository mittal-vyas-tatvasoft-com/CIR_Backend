using CIR.Core.Entities;
using CIR.Core.Entities.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.ViewModel.Utilities
{
	public class LookupsModel
	{
		public List<CulturalCodesModel> CulturalCodesList { get; set; } = new();
	}

	[NotMapped]
	public class CulturalCodesModel
	{
		public long CultureId { get; set; }
		public string CultureDisplayText { get; set; } = null!;
		public long SystemCodeId { get; set; }
		public string Code { get; set; } = null!;
	}
}
