using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Entities.Users;
using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Data.Data.GlobalConfig
{
	public class GlobalMessagesRepository : ControllerBase, IGlobalMessagesRepository
	{
		#region PROPERTIES   
		private readonly CIRDbContext _CIRDBContext;
		#endregion

		#region CONSTRUCTOR
		public GlobalMessagesRepository(CIRDbContext context)
		{
			_CIRDBContext = context ??
			   throw new ArgumentNullException(nameof(context));
		}
		#endregion

		#region METHODS

		/// <summary>
		/// This method used by GlobalConfigMessages list
		/// </summary>
		/// <returns></returns>
		public List<GlobalMessagesModel> GetGlobalMessagesList(int cultureID)
		{
			List<GlobalMessagesModel> result = new List<GlobalMessagesModel>();
			if (cultureID == 0)
			{

				result = (from globalMessages in _CIRDBContext.GlobalConfigurationMessages
						  select new GlobalMessagesModel()
						  {
							  Id = globalMessages.Id,
							  Type = globalMessages.Type,
							  Content = globalMessages.Content,
							  CultureID = globalMessages.CultureID
						  }).ToList();
			}
			else
			{
				result = (from globalMessages in _CIRDBContext.GlobalConfigurationMessages
						  select new GlobalMessagesModel()
						  {
							  Id = globalMessages.Id,
							  Type = globalMessages.Type,
							  Content = globalMessages.Content,
							  CultureID = globalMessages.CultureID
						  }).Where(x => x.CultureID == cultureID).ToList();
			}
			return result;
		}

		/// <summary>
		/// This method is used by create method and update method of globalMessage controller
		/// </summary>
		/// <param name="globalMessageModel"></param>
		/// <returns>Success status if its valid else failure</returns>
		public async Task<IActionResult> CreateOrUpdateGlobalMessages(List<GlobalMessagesModel> globalMwssageModel)
		{
			if (globalMwssageModel.Any(x => x.CultureID == 0 || x.CultureID == 0))
			{
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest});
			}
			if (globalMwssageModel != null)
			{
				foreach (var item in globalMwssageModel)
				{
					if (item.Id != 0)
					{
						GlobalMessagesModel messages = new GlobalMessagesModel()
						{
							Id = item.Id,
							Type = item.Type,
							CultureID = item.CultureID,
							Content = item.Content
						};
						_CIRDBContext.GlobalConfigurationMessages.Update(messages);
					}
					else
					{
						GlobalMessagesModel messages = new GlobalMessagesModel()
						{
							Type = item.Type,
							CultureID = item.CultureID,
							Content = item.Content
						};
						_CIRDBContext.GlobalConfigurationMessages.Add(messages);
					}
				}
				_CIRDBContext.SaveChanges();
				return new JsonResult(new CustomResponse<List<GlobalMessagesModel>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success });
			}
			return new JsonResult(new CustomResponse<GlobalMessagesModel>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound });
		}

		#endregion
	}
}
