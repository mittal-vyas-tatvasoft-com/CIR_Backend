﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Entities.Utilities.SystemSettings
{
	public partial class LookupItemsText
	{
		public long Id { get; set; }

		public long LookupItemId { get; set; }

		public long CultureId { get; set; }

		public int DisplayOrder { get; set; }

		public string Text { get; set; } = null!;

		public bool Active { get; set; }
	}

	
}


