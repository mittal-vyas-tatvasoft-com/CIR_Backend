using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIR.Core.Entities;
using CIR.Core.Interfaces.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.Utilities
{
    public class CultureService : ICultureService
    {
        private readonly ICultureRepository _cultureRepository;

        public CultureService(ICultureRepository cultureRepository)
        {
            _cultureRepository = cultureRepository;
        }
      
        public async Task<List<Culture>> GetAllCultures()
        {
            var cultures = _cultureRepository.GetAllCultures();
            return await cultures;
        }
    }
}
