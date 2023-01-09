using CIR.Core.Entities;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.GlobalConfig
{
    public interface ICutOffTimesRepository
    {
        Task<GlobalConfigurationCutOffTimeModel> GetCutOffTimeAndDayById(int id);
        Task<IActionResult> SaveCutOffTime (GlobalConfigurationCutOffTimeModel model);
    }
}
