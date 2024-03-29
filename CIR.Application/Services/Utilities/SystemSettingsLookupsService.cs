﻿using CIR.Core.Interfaces.Utilities;
using CIR.Core.ViewModel.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.Utilities
{
    public class SystemSettingsLookupsService : ISystemSettingsLookupsService
    {
        #region PROPERTIES
        private readonly ILookupsRepository _lookupsRepository;
        #endregion

        #region CONSTRUCTOR
        public SystemSettingsLookupsService(ILookupsRepository lookupsRepository)
        {
            _lookupsRepository = lookupsRepository;
        }
        #endregion

        #region METHODS

        public Task<IActionResult> CreateOrUpdateLookupItem(LookupItemsTextModel lookupItemsTextModel)
        {
            return _lookupsRepository.CreateOrUpdateLookupItem(lookupItemsTextModel);
        }

        public async Task<bool> LookupItemExists(long cultureId, long lookupItemId)
        {
            return await _lookupsRepository.LookupItemExists(cultureId, lookupItemId);
        }

        public Task<IActionResult> GetAllCultureCodeList(long? cultureId, string? code, string? sortCol, string? searchCultureCode, bool sortAscending = true)
        {
            return _lookupsRepository.GetAllCultureCodeList(cultureId, code, sortCol, searchCultureCode, sortAscending);
        }

        public Task<IActionResult> GetAllLookupsItems(long cultureId, string code, string? searchLookupItems, bool sortAscending = true)
        {
            return _lookupsRepository.GetAllLookupsItems(cultureId, code, searchLookupItems, sortAscending);
        }

        public Task<IActionResult> GetLookupById(int id)
        {
            return _lookupsRepository.GetLookupById(id);
        }
        #endregion
    }
}
