using CIR.Core.ViewModel.Websites;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Websites
{
	public interface IPortal2GlobalConfigurationReasonsServices
	{
		Task<IActionResult> CreateReason(List<Portal2GlobalConfigurationReasonsModel> ReasonsModel);
		Task<IActionResult> GetAllReasons();
	}
}
