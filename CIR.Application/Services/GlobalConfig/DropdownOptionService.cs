using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.GlobalConfig
{
    public class DropdownOptionService : IDropdownOptionService
    {
        private readonly IDropdownOptionRepository _dropdownOptionRepository;

        public DropdownOptionService(IDropdownOptionRepository dropdownOptionRepository)
        {
            _dropdownOptionRepository = dropdownOptionRepository;
        }

        public async Task<IActionResult> GetAllDropdownOptions()
        {
            return await _dropdownOptionRepository.GetAllDropdownOptions();
        }

        public async Task<IActionResult> CreateOrUpdateDrownOption(List<GlobalConfigurationReasons> dropdownOptions)
        {
            return await _dropdownOptionRepository.CreateOrUpdateDrownOption(dropdownOptions);
        }
    }
}
