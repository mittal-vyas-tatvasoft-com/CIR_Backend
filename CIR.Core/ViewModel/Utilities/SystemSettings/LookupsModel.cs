using CIR.Core.Entities;
using CIR.Core.Entities.Utilities.SystemSettings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.ViewModel.Utilities.SystemSettings
{
	public class LookupsModel
	{
		public long Id { get; set; }

		public long LookupItemId { get; set; }

		public long CultureId { get; set; }

		public int DisplayOrder { get; set; }

		public string Text { get; set; } = null!;

		public bool Active { get; set; }

		public long SystemCodeId { get; set; }

		public string Code { get; set; } = null!;
		public List<LookupItemsText> LookupItemsList { get; set; } = new();
		public List<CulturalCodesModel> CulturalCodesList { get; set; } = new();
		public List<SystemCode> SystemCodeList { get; set; } = new();
		public string CultureName { get; set; } = null!;
	}

	[NotMapped]
	public class CulturalCodesModel
	{
		public long CultureId { get; set; }
		public string Code { get; set; } = null!;
		public long SystemCodeId { get; set; }
		public string CultureDisplayText { get; set; } = null!;

	}
}
