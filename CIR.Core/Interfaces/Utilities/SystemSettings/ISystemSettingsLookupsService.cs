using CIR.Core.ViewModel;
using CIR.Core.ViewModel.Utilities.SystemSettings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.Utilities.SystemSettings
{
	public interface ISystemSettingsLookupsService
	{
		Task<IActionResult> CreateOrUpdateLookupItem(LookupsModel lookupModel);

		Task<Boolean> LookupItemExists(long cultureId, long lookupItemId);

		Task<LookupsModel> GetAllLookupsItems(string sortCol, string? searchCultureCode, string? searchLookupItems, bool sortAscending = true);

	}
}
