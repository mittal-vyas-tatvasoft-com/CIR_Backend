using CIR.Core.Entities.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfiguration
{
	public interface IGlobalConfigurationFieldServices
	{
		Task<IActionResult> GetAllFields();
		Task<IActionResult> CreateOrUpdateFields(List<GlobalConfigurationField> globalConfigurationFields);
	}
}
