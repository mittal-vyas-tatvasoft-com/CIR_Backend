using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.GlobalConfiguration
{
	public class GlobalConfigurationFieldService : IGlobalConfigurationFieldServices
	{
		private readonly IGlobalConfigurationFieldRepository _globalConfigurationFieldRepository;

		public GlobalConfigurationFieldService(IGlobalConfigurationFieldRepository globalConfigurationFieldRepository)
		{
			_globalConfigurationFieldRepository = globalConfigurationFieldRepository;
		}

		public async Task<IActionResult> GetAllFields()
		{
			return await _globalConfigurationFieldRepository.GetAllFields();
		}

		public async Task<IActionResult> CreateOrUpdateFields(List<GlobalConfigurationField> globalConfigurationFields)
		{
			return await _globalConfigurationFieldRepository.CreateOrUpdateFields(globalConfigurationFields);
		}
	}
}
