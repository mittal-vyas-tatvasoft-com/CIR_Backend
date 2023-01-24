using CIR.Common.CustomResponse;
using CIR.Core.Entities.Website;
using CIR.Core.Interfaces.Website;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.Website
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PortalToGlobalConfigurationEmailsController : ControllerBase
    {
        #region PROPERTIES
        private readonly IPortalToGlobalConfigurationEmailsService _portalToGlobalConfigurationEmailsService;
        #endregion

        #region CONSTRUCTORS
        public PortalToGlobalConfigurationEmailsController(IPortalToGlobalConfigurationEmailsService portalToGlobalConfigurationEmailsService)
        {
            _portalToGlobalConfigurationEmailsService = portalToGlobalConfigurationEmailsService;
        }
        #endregion

        #region METHODS

        /// <summary>
		/// This method takes a update website portalToGlobalConfigurationEmails
		/// </summary>
		/// <param name="portalToGlobalConfigurationEmails"></param>
		/// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Post(List<PortalToGlobalConfigurationEmails> portalToGlobalConfigurationEmails)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await _portalToGlobalConfigurationEmailsService.CreateOrUpdatePortalToGlobalConfigurationEmails(portalToGlobalConfigurationEmails);

                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = SystemMessages.msgBadRequest });

        }
        /// <summary>
		/// This method takes a get website portalToGlobalConfigurationEmails list
		/// </summary>
        /// <param name="id">this method returns details of given id</param>
		/// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return await _portalToGlobalConfigurationEmailsService.GetPortalToGlobalConfigurationEmailsList(id);
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }
        #endregion
    }
}
