using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.Interfaces.Users;
using CIR.Core.ViewModel.GlobalConfig;
using CIR.Core.ViewModel.Usersvm;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Application.Services.GlobalConfig
{
    public class StylesService:IStylesService
    {
        private readonly IStylesRepository _stylesRepository;

        public StylesService(IStylesRepository stylesRepository)
        {
            _stylesRepository = stylesRepository;
        }
        public Task<GlobalConfigurationStyleModel> GetAllStyles()
        {
            return _stylesRepository.GetAllStyles();
        }
        public Task<IActionResult> SaveStyle(List<GlobalConfigurationStyle> model)
        {
            return _stylesRepository.SaveStyle(model);
        }
    }
}
