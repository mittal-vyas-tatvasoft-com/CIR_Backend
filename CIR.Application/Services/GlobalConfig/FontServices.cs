using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.GlobalConfig
{
	public class FontServices : IFontServices
	{
		private readonly IFontRepository _fontRepository;
		public FontServices(IFontRepository fontRepository)
		{
			_fontRepository = fontRepository;
		}
		public async Task<IActionResult> GetAllFonts()
		{
			var fonts = await _fontRepository.GetAllFonts();
			return fonts;
		}
		public Task<IActionResult> CreateOrUpdateFonts(List<GlobalConfigurationFonts> fonts)
		{
			return _fontRepository.CreateOrUpdateFont(fonts);
		}
	}
}
