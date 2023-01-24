using CIR.Core.Interfaces.Websites;
using CIR.Core.ViewModel.Websites;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.Websites
{
    public class Portal2GlobalConfigurationReasonsServices : IPortal2GlobalConfigurationReasonsServices
    {
        private readonly IPortal2GlobalConfigurationReasonsRepository _portal2GlobalConfigurationReasonsRepository;
        public Portal2GlobalConfigurationReasonsServices(IPortal2GlobalConfigurationReasonsRepository portal2GlobalConfigurationReasonsRepository)
        {
            _portal2GlobalConfigurationReasonsRepository = portal2GlobalConfigurationReasonsRepository;
        }
        public async Task<IActionResult> CreateReason(List<Portal2GlobalConfigurationReasonsModel> ReasonsModel)
        {
            return await _portal2GlobalConfigurationReasonsRepository.CreateReason(ReasonsModel);
        }
        public async Task<IActionResult> GetAllReasons()
        {
            return await _portal2GlobalConfigurationReasonsRepository.GetAllReasons();
        }

    }
}
