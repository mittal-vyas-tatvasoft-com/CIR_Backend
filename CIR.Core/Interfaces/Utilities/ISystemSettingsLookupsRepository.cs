using CIR.Core.ViewModel.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Utilities
{
    public interface ILookupsRepository
    {
        Task<IActionResult> CreateOrUpdateLookupItem(LookupItemsTextModel lookupItemsTextModel);
        Task<IActionResult> GetAllCultureCodeList(long? cultureId, string? code, string? sortCol, string? searchCultureCode, bool sortAscending = true);
        Task<bool> LookupItemExists(long cultureId, long lookupItemId);
        Task<IActionResult> GetAllLookupsItems(long cultureId, string code, string? searchLookupItems, bool sortAscending = true);
        Task<IActionResult> GetLookupById(int id);
    }
}
