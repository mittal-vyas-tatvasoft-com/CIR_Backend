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
    public class ClientsController : ControllerBase
    {
        #region PROPERTIES

        private readonly IClientService _clientService;

        #endregion

        #region CONSTRUCTORS

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        #endregion

        #region METHODS

        /// <summary>
        /// This method fetched the list of clients
        /// </summary>
        /// <returns>list of the clients</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return await _clientService.GetAllClients();
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        /// <summary>
        /// This method takes client details and adds it
        /// </summary>
        /// <param name="clientModel"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ClientModel clientModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await _clientService.CreateOrUpdateClient(clientModel);
                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = SystemMessages.msgBadRequest });
        }

        /// <summary>
        /// This method takes client details and updates it
        /// </summary>
        /// <param name="clientModel"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public async Task<IActionResult> Update(ClientModel clientModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await _clientService.CreateOrUpdateClient(clientModel);
                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = SystemMessages.msgBadRequest });
        }

        #endregion
    }
}
