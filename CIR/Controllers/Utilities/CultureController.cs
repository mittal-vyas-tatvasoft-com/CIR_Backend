using Microsoft.AspNetCore.Mvc;
using CIR.Core.Interfaces.Utilities;
using Microsoft.AspNetCore.Authorization;
using CIR.Common.CustomResponse;
using CIR.Core.Entities;

namespace CIR.Controllers.Utilities
{
    [Route("api/Culture")]
    [ApiController]
    [Authorize]
    public class CultureController : ControllerBase
    {
        private readonly ICultureService _cultureService;

        public CultureController(ICultureService cultureService)
        {
            _cultureService = cultureService;
        }

        [HttpGet]
        public async Task<CustomResponse<List<Culture>>> GetAll()
        {
            try
            {
                var cultures = await _cultureService.GetAllCultures();
                if(cultures.Count > 0)
                {
                    return new CustomResponse<List<Culture>> { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = cultures };
                }
                else
                {
                    return new CustomResponse<List<Culture>>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = cultures };
                }
            }
            catch(Exception e)
            {
                return new CustomResponse<List<Culture>>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError };
            }
        }

    }
}
