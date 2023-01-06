using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.GlobalConfig
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlobalCurrencyController : ControllerBase
    {
        private readonly IGlobalCurrencyService _currencyService;
        public GlobalCurrencyController(IGlobalCurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        /// <summary>
        /// This method takes get global currency country wise
        /// </summary>
        /// <param name="countryId">this object contains countryId</param>
        /// <returns></returns>
        [HttpGet("{countryId}")]
        public async Task<IActionResult> Get(int countryId)
        {
            try
            {
                var currencyList = _currencyService.GetCurrencyCountryWise(countryId);
                return Ok(currencyList);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error : " + ex });
            }
        }

        /// <summary>
        /// This method takes add global currency
        /// </summary>
        /// <param name="globalCurrencyModel">this object contains different parameters as details of a globalcurrency</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(List<GlobalCurrencyModel> globalCurrencyModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var addGlobalCurrency = _currencyService.CreateOrUpdateGlobalCurrencies(globalCurrencyModel);
                    if (addGlobalCurrency == "Success")
                    {
                        return Ok(new { message = addGlobalCurrency });
                    }
                    else
                    {
                        return BadRequest(new { message = "Error occured add new global currency" });
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = "Error : " + ex + " Invalid input data" });
                }

            }
            return BadRequest();
        }

        /// <summary>
        /// This method takes update global currency
        /// </summary>
        /// <param name="globalCurrencyModel">this object contains different parameters as details of a globalcurrency</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put(List<GlobalCurrencyModel> globalCurrencyModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updateGlobalCurrency = _currencyService.CreateOrUpdateGlobalCurrencies(globalCurrencyModel);
                    if (updateGlobalCurrency == "Success")
                    {
                        return Ok(new { message = updateGlobalCurrency });
                    }
                    else
                    {
                        return BadRequest(new { message = "Error occured update global currency" });
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = "Error : " + ex + " Invalid input data" });
                }
            }
            return BadRequest();
        }
    }
}
