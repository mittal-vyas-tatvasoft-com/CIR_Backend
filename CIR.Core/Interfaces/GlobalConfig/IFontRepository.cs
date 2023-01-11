using CIR.Core.Entities.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfig
{
	public interface IFontRepository
	{
		Task<IActionResult> GetAllFonts();
		Task<IActionResult> CreateOrUpdateFont(List<GlobalConfigurationFonts> fonts);
	}
}
