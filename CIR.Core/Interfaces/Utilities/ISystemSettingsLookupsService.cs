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
        Task<IActionResult> CreateOrUpdateLookupItem(LookupsModel lookupModel);

        Task<bool> LookupItemExists(long cultureId, long lookupItemId);

        Task<LookupsModel> GetAllLookupsItems(long cultureId, string code, string sortCol, string? searchCultureCode, string? searchLookupItems, bool sortAscending = true);

    }
}
