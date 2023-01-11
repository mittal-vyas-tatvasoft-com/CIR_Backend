using CIR.Core.Entities.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfig
{
    public interface IDropdownOptionRepository
    {
        Task<IActionResult> GetAllDropdownOptions();
        Task<IActionResult> CreateOrUpdateDrownOption(List<GlobalConfigurationReasons> dropdownOptions);
    }
}
