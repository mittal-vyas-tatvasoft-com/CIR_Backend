using CIR.Core.Entities.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfig
{
	public interface IFontServices
	{
		Task<IActionResult> GetAllFonts();
		Task<IActionResult> CreateOrUpdateFonts(List<GlobalConfigurationFonts> fonts);
	}
}
