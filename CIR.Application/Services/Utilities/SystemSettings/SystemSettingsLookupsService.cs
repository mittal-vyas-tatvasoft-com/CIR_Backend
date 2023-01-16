using CIR.Core.Entities;
using CIR.Core.Entities.Users;
using CIR.Core.Interfaces.Users;
using CIR.Core.Interfaces.Utilities.SystemSettings;
using CIR.Core.ViewModel.Utilities.SystemSettings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Application.Services.Utilities.SystemSettings
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

		public async Task<Boolean> LookupItemExists(long cultureId, long lookupItemId)
		{
			return await _lookupsRepository.LookupItemExists(cultureId, lookupItemId);
		}
		public Task<LookupsModel> GetAllLookupsItems(string sortCol, string? searchCultureCode, string? searchLookupItems, bool sortAscending = true)
		{
			return _lookupsRepository.GetAllLookupsItems(sortCol, searchCultureCode, searchLookupItems, sortAscending);
		}

		#endregion
	}
}
