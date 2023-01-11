using CIR.Core.Entities;
using CIR.Core.Interfaces.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.ViewModel.GlobalConfig;

namespace CIR.Application.Services.GlobalConfig
{
    public class CutOffTimesService : ICutOffTimesService
    {
        private readonly ICutOffTimesRepository _cutOffTimeRepository;

        public CutOffTimesService(ICutOffTimesRepository cutOffTimeRepository)
        {
            _cutOffTimeRepository = cutOffTimeRepository;
        }
        public async Task<IActionResult> GetCutOffTimeAndDayById(int id)
        {
            var data = await _cutOffTimeRepository.GetCutOffTimeAndDayById(id);
            return data;
        }
        public Task<IActionResult> SaveCutOffTime(GlobalConfigurationCutOffTimeModel model)
        {
            return _cutOffTimeRepository.SaveCutOffTime(model);
        }
        
    }
}
