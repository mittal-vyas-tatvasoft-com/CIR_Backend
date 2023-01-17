using CIR.Core.ViewModel.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.Utilities
{
    public interface ILookupsRepository
    {
        Task<IActionResult> CreateOrUpdateLookupItem(LookupsModel lookupModel);
        Task<LookupsModel> GetAllLookupsItems(long cultureId, string code, string sortCol, string? searchCultureCode, string? searchLookupItems, bool sortAscending = true);
        Task<bool> LookupItemExists(long cultureId, long lookupItemId);
    }
}
