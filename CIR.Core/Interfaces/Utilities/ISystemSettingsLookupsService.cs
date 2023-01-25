using CIR.Core.ViewModel.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Utilities
{
    public interface ISystemSettingsLookupsService
    {
        Task<IActionResult> CreateOrUpdateLookupItem(LookupItemsTextModel lookupItemsTextmodel);
        Task<bool> LookupItemExists(long cultureId, long lookupItemId);
        Task<IActionResult> GetAllCultureCodeList(long? cultureId, string? code, string? sortCol, string? searchCultureCode, bool sortAscending = true);
        Task<IActionResult> GetAllLookupsItems(long cultureId, string code, string? searchLookupItems, bool sortAscending = true);
        Task<IActionResult> GetLookupById(int id);
    }
}
