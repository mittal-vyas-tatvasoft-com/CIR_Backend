using CIR.Common.CustomResponse;
using CIR.Core.Entities.Users;
using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.GlobalConfig
{
	[Route("api/[controller]")]
	[ApiController]
	public class GlobalMessagesController : ControllerBase
	{
		#region PROPERTIES

		private readonly IGlobalMessagesService _globalMessages;

		#endregion

		#region CONSTRUCTORS

		public GlobalMessagesController(IGlobalMessagesService globalMessages)
		{
			_globalMessages = globalMessages;
		}

		#endregion

		#region METHODS
		/// <summary>
		/// This method fetches single message data using cultureID's Id
		/// </summary>
		/// <param name="id">messages will be fetched according to this 'id'</param>
		/// <returns> Messages </returns> 

		[HttpGet("{cultureID}")]
		public async Task<IActionResult> Get(int cultureID)
		{
			try
			{
				var messageList = _globalMessages.GetGlobalMessagesList(cultureID);
				if (messageList != null && messageList.Count>0)
				{ 
					return new JsonResult(new CustomResponse<List<GlobalMessagesModel>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = messageList });
				}
				return new JsonResult(new CustomResponse<List<GlobalMessagesModel>>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = messageList });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
			}
		}

		/// <summary>
		/// This method takes add global Messages
		/// </summary>
		/// <param name="globalMessageModel">this object contains different parameters as details of a globalMessages</param>
		/// <returns></returns>
		[HttpPost("{id}")]
		public async Task<IActionResult> Post(List<GlobalMessagesModel> globalMessageModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					return await _globalMessages.CreateOrUpdateGlobalMessages(globalMessageModel);
				}
				catch (Exception ex)
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
				}

			}
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });
		}
		#endregion
	}
}
