using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.GlobalConfiguration
{
	public class GlobalConfigurationFontsServices : IGlobalConfigurationFontsServices
	{
		private readonly IGlobalConfigurationFontsRepository _globalConfigurationFontsRepository;
		public GlobalConfigurationFontsServices(IGlobalConfigurationFontsRepository globalConfigurationFontsRepository)
		{
			_globalConfigurationFontsRepository = globalConfigurationFontsRepository;
		}
		public async Task<IActionResult> GetGlobalConfigurationFonts()
		{
			return await _globalConfigurationFontsRepository.GetGlobalConfigurationFonts();
		}
		public async Task<IActionResult> CreateOrUpdateGlobalConfigurationFonts(List<GlobalConfigurationFonts> globalConfigurationFonts)
		{
			return await _globalConfigurationFontsRepository.CreateOrUpdateGlobalConfigurationFonts(globalConfigurationFonts);
		}
	}
}
