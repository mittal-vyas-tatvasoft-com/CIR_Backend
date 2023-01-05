using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIR.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Utilities
{
    public interface ICultureRepository
    {
        public Task<List<Culture>> GetAllCultures();
    }
}
