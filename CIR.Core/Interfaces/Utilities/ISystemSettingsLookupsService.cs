using CIR.Core.Entities.Utilities;
using CIR.Core.ViewModel;
using CIR.Core.ViewModel.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.Utilities
{
	public interface ISystemSettingsLookupsService
	{
		Task<IActionResult> CreateOrUpdateLookupItem(LookupItemsTextModel lookupItemsTextmodel);
		Task<bool> LookupItemExists(long cultureId, long lookupItemId);
		Task<IActionResult> GetAllCultureCodeList(long? cultureId, string? code, string? sortCol, string? searchCultureCode, bool sortAscending = true);
		Task<IActionResult> GetAllLookupsItems(long cultureId, string code,  string? searchLookupItems, bool sortAscending = true);

	}
}
