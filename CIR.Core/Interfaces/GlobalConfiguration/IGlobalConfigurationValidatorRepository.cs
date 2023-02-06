using CIR.Core.Entities.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfiguration
{
	public interface IGlobalConfigurationValidatorRepository
	{
		Task<IActionResult> GetGlobalConfigurationValidators();
		Task<IActionResult> CreateOrUpdateGlobalConfigurationValidators(List<GlobalConfigurationValidator> globalConfigurationValidators);
	}
}
