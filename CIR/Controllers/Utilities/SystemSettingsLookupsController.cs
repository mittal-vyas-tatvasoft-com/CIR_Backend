using CIR.Application.Services.Users;
using CIR.Common.CustomResponse;
using CIR.Controllers.Users;
using CIR.Core.Entities.Users;
using CIR.Core.Interfaces.Users;
using CIR.Core.Interfaces.Utilities;
using CIR.Core.ViewModel.Usersvm;
using CIR.Core.ViewModel.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.Utilities
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SystemSettingsLookupsController : ControllerBase
    {
        #region PROPERTIES
        private readonly ISystemSettingsLookupsService _lookupService;
        private readonly ILogger<UsersController> _logger;
        #endregion

        #region CONSTRUCTOR
        public SystemSettingsLookupsController(ISystemSettingsLookupsService lookupService, ILogger<UsersController> logger)
        {
            _lookupService = lookupService;
            _logger = logger;
        }
        #endregion

        #region METHODS
        /// <summary>
        /// This method takes Lookups details and updates the LookupItem
        /// </summary>
        /// <param name="lookupModel"> this object contains different parameters as details of a lookupItem </param>
        /// <returns> updated LookupItems </returns>

        [HttpPut("[action]")]
        public async Task<IActionResult> Post(LookupsModel lookupsModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var isExist = await _lookupService.LookupItemExists(lookupsModel.CultureId, lookupsModel.LookupItemId);
                    if (isExist)
                    {
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Lookup Item already exists" });
                    }
                    else
                    {
                        return await _lookupService.CreateOrUpdateLookupItem(lookupsModel);
                    }
                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Error" });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Get(long cultureId, string code, string sortCol, string? searchCultureCode, string? searchLookupItems, bool sortAscending = true)
        {
            try
            {
                searchCultureCode ??= string.Empty;
                searchLookupItems ??= string.Empty;

                var lookupModel = await _lookupService.GetAllLookupsItems(cultureId, code, sortCol, searchCultureCode, searchLookupItems, sortAscending);

                if (lookupModel != null)
                {
                    return new JsonResult(new CustomResponse<LookupsModel>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = lookupModel });
                }
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = "Requested Item were not found" });
            }
            catch (Exception ex)
            {

                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        #endregion
    }
}
