using CIR.Core.Entities.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfig
{
	public interface IGlobalConfigurationFontsRepository
	{
		Task<IActionResult> GetGlobalConfigurationFonts();
		Task<IActionResult> CreateOrUpdateGlobalConfigurationFonts(List<GlobalConfigurationFonts> globalConfigurationFonts);
	}
}
