using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.GlobalConfiguration
{
	public class GlobalConfigurationValidatorService : IGlobalConfigurationValidatorService
	{
		private readonly IGlobalConfigurationValidatorRepository _globalConfigurationValidatorRepository;
		public GlobalConfigurationValidatorService(IGlobalConfigurationValidatorRepository globalConfigurationValidatorRepository)
		{
			_globalConfigurationValidatorRepository = globalConfigurationValidatorRepository;
		}
		public async Task<IActionResult> GetGlobalConfigurationValidators()
		{
			return await _globalConfigurationValidatorRepository.GetGlobalConfigurationValidators();
		}
		public async Task<IActionResult> CreateOrUpdateGlobalConfigurationValidators(List<GlobalConfigurationValidator> globalConfigurationValidators)
		{
			return await _globalConfigurationValidatorRepository.CreateOrUpdateGlobalConfigurationValidators(globalConfigurationValidators);
		}
	}
}

