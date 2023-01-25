using CIR.Common.CustomResponse;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities.Websites;
using CIR.Core.Interfaces.Websites;
using CIR.Core.ViewModel.Websites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.Websites
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class Portal2GlobalConfigurationReasonsController : ControllerBase
    {
        #region PROPERTIES
        private readonly IPortal2GlobalConfigurationReasonsServices _portal2GlobalConfigurationReasonsServices;
        #endregion

        #region CONSTRUCTOR
        public Portal2GlobalConfigurationReasonsController(IPortal2GlobalConfigurationReasonsServices portal2GlobalConfigurationReasonsServices)
        {
            _portal2GlobalConfigurationReasonsServices = portal2GlobalConfigurationReasonsServices;
        }
        #endregion


        #region METHODS

		/// <summary>
		/// This method fetched the list of Reasons
		/// </summary>
		/// <returns>list of the reasons</returns>
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				return await _portal2GlobalConfigurationReasonsServices.GetAllReasons();
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method takes reasons details and adds it
		/// </summary>
		/// <param name="portal2GlobalConfigurationReasonsModels"></param>
		/// <returns></returns>
		/// 
		[HttpPost("Create")]
		public async Task<IActionResult> Create(List<Portal2GlobalConfigurationReasonsModel> portal2GlobalConfigurationReasonsModels)
		{
			if (ModelState.IsValid)
			{
				try
				{
					return await _portal2GlobalConfigurationReasonsServices.CreateReason(portal2GlobalConfigurationReasonsModels);
				}
				catch (Exception ex)
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
				}
			}
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });
		}
		#endregion
	}
}
