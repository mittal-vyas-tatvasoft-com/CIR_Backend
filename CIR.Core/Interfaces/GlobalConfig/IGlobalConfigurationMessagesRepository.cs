﻿using CIR.Core.Entities.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfig
{
    public interface IGlobalConfigurationMessagesRepository
    {
        Task<IActionResult> GetGlobalConfigurationMessagesList(int cultureId);
        Task<IActionResult> CreateOrUpdateGlobalConfigurationMessages(List<GlobalConfigurationMessages> globalConfigurationMessages);
    }
}
