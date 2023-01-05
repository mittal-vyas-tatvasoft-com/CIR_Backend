using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIR.Common.Data;
using CIR.Core.Entities;
using CIR.Core.Interfaces.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CIR.Data.Data
{
    public class CultureRepository : ICultureRepository
    {
        private readonly CIRDbContext _CIRDBContext;

        public CultureRepository(CIRDbContext context)
        {
            _CIRDBContext = context ??
               throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Culture>> GetAllCultures()
        {
            var cultures = await _CIRDBContext.Cultures.ToListAsync();
            return cultures;
        }
    }
}
