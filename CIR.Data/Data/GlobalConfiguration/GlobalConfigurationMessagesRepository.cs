using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using CIR.Core.ViewModel.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CIR.Data.Data.GlobalConfiguration
{
    public class GlobalConfigurationMessagesRepository : ControllerBase, IGlobalConfigurationMessagesRepository
	{
		#region PROPERTIES   
		private readonly CIRDbContext _CIRDBContext;
		#endregion

		#region CONSTRUCTOR
		public GlobalConfigurationMessagesRepository(CIRDbContext context)
		{
			_CIRDBContext = context ??
			   throw new ArgumentNullException(nameof(context));
		}
		#endregion

		#region METHODS

		/// <summary>
		/// This method used by get globalconfiguration messages list
		/// </summary>
		/// <param name="cultureId"></param>
		/// <returns></returns>
		public async Task<IActionResult> GetGlobalConfigurationMessagesList(int cultureId)
		{
			try
			{

				var globalConfigurationMessagesList = await (from globalMessages in _CIRDBContext.GlobalConfigurationMessages
															 join culture in _CIRDBContext.Cultures on globalMessages.CultureId equals culture.Id
															 select new GlobalMessagesModel()
															 {
																 Id = globalMessages.Id,
																 Type = globalMessages.Type,
																 Content = globalMessages.Content,
																 CultureId = globalMessages.CultureId,
																 CultureName = culture.Name
															 }).Where(x => x.CultureId == cultureId).ToListAsync();

				if (globalConfigurationMessagesList.Count == 0)
				{
					return new JsonResult(new CustomResponse<List<GlobalConfigurationReasons>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}
				return new JsonResult(new CustomResponse<List<GlobalMessagesModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = globalConfigurationMessagesList });

			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method is used by create or update globalConfiguration Messages
		/// </summary>
		/// <param name="globalConfigurationMessages"></param>
		/// <returns>Success status if its valid else failure</returns>
		public async Task<IActionResult> CreateOrUpdateGlobalConfigurationMessages(List<GlobalConfigurationMessages> globalConfigurationMessages)
		{
			try
			{
				if (globalConfigurationMessages != null)
				{
					foreach (var item in globalConfigurationMessages)
					{
						if (item.Id != 0)
						{
							GlobalConfigurationMessages globalConfigMessage = new GlobalConfigurationMessages()
							{
								Id = item.Id,
								Type = item.Type,
								CultureId = item.CultureId,
								Content = item.Content
							};
							_CIRDBContext.GlobalConfigurationMessages.Update(globalConfigMessage);
						}
						else
						{
							GlobalConfigurationMessages globalConfigMessage = new GlobalConfigurationMessages()
							{
								Type = item.Type,
								CultureId = item.CultureId,
								Content = item.Content
							};
							_CIRDBContext.GlobalConfigurationMessages.Add(globalConfigMessage);
						}
					}
					await _CIRDBContext.SaveChangesAsync();
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataSavedSuccessfully, "GlobalConfiguration Messages") });
				}
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		#endregion
	}
}
