using CIR.Common.CustomResponse;
using CIR.Core.Interfaces.Website;
using CIR.Core.ViewModel.Website;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.Website
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PortalsController : ControllerBase
    {
        #region PROPERTIES

        private readonly IPortalService _portalService;

        #endregion


        #region CONSTRUCTORS

        public PortalsController(IPortalService portalService)
        {
            _portalService = portalService;
        }

        #endregion


        #region METHODS

        /// <summary>
        /// This method takes all the portal details and adds it
        /// </summary>
        /// <param name="portalModel"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpPost("Create/{clientId}")]
        public async Task<IActionResult> Create(PortalModel portalModel, long clientId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await _portalService.CreatePortal(portalModel, clientId);
                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });
        }

        /// <summary>
        /// This method disbles the portal of given portal Id
        /// </summary>
        /// <param name="portalId"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{portalId}")]
        public async Task<IActionResult> Delete(long portalId)
        {
            try
            {
                return await _portalService.DisablePortal(portalId);
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        #endregion
    }
}
