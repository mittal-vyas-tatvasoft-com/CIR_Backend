using CIR.Core.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ICommonService _commonService;
        public CommonController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        /// <summary>
        /// This method get a currencies list
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCurrencies")]
        public async Task<IActionResult> GetCurrencies()
        {
            if (ModelState.IsValid)
            {
                var currencyList = _commonService.GetCurrencies();
                return Ok(currencyList);
            }
            return BadRequest();
        }

        /// <summary>
        /// This method get a country list
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCountry")]
        public async Task<IActionResult> GetCountry()
        {
            if (ModelState.IsValid)
            {
                var countryList = _commonService.GetCountry();
                return Ok(countryList);
            }
            return BadRequest();
        }
    }
}
