using CIR.Core.Entities;
using CIR.Core.Entities.Users;
using CIR.Core.Interfaces.Users;
using CIR.Core.Interfaces.Utilities;
using CIR.Core.ViewModel.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task<IActionResult> CreateOrUpdateLookupItem(LookupsModel lookupModel)
        {
            return _lookupsRepository.CreateOrUpdateLookupItem(lookupModel);
        }

        public async Task<bool> LookupItemExists(long cultureId, long lookupItemId)
        {
            return await _lookupsRepository.LookupItemExists(cultureId, lookupItemId);
        }
        public Task<LookupsModel> GetAllLookupsItems(long cultureId, string code, string sortCol, string? searchCultureCode, string? searchLookupItems, bool sortAscending = true)
        {
            return _lookupsRepository.GetAllLookupsItems(cultureId, code, sortCol, searchCultureCode, searchLookupItems, sortAscending);
        }

        #endregion
    }
}
