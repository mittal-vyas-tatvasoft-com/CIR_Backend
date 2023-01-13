using CIR.Common.CustomResponse;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Entities.Users;
using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.GlobalConfig
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlobalEmailController : ControllerBase
    {
        #region PROPERTIES

        private readonly IGlobalEmailsService _globalEmailsService;

        #endregion

        #region CONSTRUCTORS
        public GlobalEmailController(IGlobalEmailsService emailService)
        {
            _globalEmailsService = emailService;
        }
        #endregion


        #region METHODS
        /// <summary>
		/// This method takes Email details as parameters and give details.
		/// </summary>
		/// <param name="globalEmailsModel"> this object contains different parameters as details of a email</param>
		/// <returns >update email</returns>
        [HttpPost]
        public async Task<IActionResult> Post(List<GlobalConfigurationEmailsModel> globalEmailsModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await _globalEmailsService.CreateOrUpdateGlobalEmails(globalEmailsModel);

                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });

        }

        /// <summary>
        /// This method takes get detail about email by id wise
        /// </summary>
        /// <param name="Id">this object contains id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return await _globalEmailsService.GetGlobalEmailsDataList(id);
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }
        #endregion
    }
}
