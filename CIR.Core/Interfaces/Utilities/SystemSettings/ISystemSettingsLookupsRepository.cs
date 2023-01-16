using CIR.Core.ViewModel.Utilities.SystemSettings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.Utilities.SystemSettings
{
    public interface ILookupsRepository
    {
		Task<IActionResult> CreateOrUpdateLookupItem(LookupsModel lookupModel);
		Task<LookupsModel> GetAllLookupsItems( string sortCol, string? searchCultureCode, string? searchLookupItems, bool sortAscending = true);
		Task<Boolean> LookupItemExists(long cultureId, long lookupItemId);
	}
}
