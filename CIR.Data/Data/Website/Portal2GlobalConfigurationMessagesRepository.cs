using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities.Website;
using CIR.Core.Interfaces.Website;
using CIR.Core.ViewModel.Website;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CIR.Data.Data.Website
{
	public class Portal2GlobalConfigurationMessagesRepository : ControllerBase, IPortal2GlobalConfigurationMessagesRepository
	{
		#region PROPERTIES
		private readonly CIRDbContext _CIRDbContext;
		#endregion

		#region CONSTRUCTOR
		public Portal2GlobalConfigurationMessagesRepository(CIRDbContext context)
		{
			_CIRDbContext = context ??
				throw new ArgumentNullException(nameof(context));
		}
		#endregion

		#region METHODS
		/// <summary>
		/// This method used by PortalToGlobalConfigurationMessagesList
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> GetPortalToGlobalConfigurationMessagesList(long portalId)
		{
			try
			{
				List<Portal2GlobalConfigurationMessagesModel> portal2GlobalConfigurationMessagesModels = new();
				using (DbConnection dbConnection = new DbConnection())
				{
					using (var connection = dbConnection.Connection)
					{
						DynamicParameters parameters = new DynamicParameters();
						parameters.Add("@portalId", portalId);
						portal2GlobalConfigurationMessagesModels = connection.Query<Portal2GlobalConfigurationMessagesModel>("spGetPortalToGlobalConfigurationMessagesPortalWise", parameters, commandType: CommandType.StoredProcedure).ToList();
					}
				}
				if (portal2GlobalConfigurationMessagesModels.Count == 0)
				{
					return new JsonResult(new CustomResponse<List<Portal2GlobalConfigurationMessagesModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}
				return new JsonResult(new CustomResponse<List<Portal2GlobalConfigurationMessagesModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = portal2GlobalConfigurationMessagesModels });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method is used by create method and update method of PortalToGlobalConfigurationMessages controller
		/// </summary>
		/// <param name="portalToGlobalConfigurationMessages"></param>
		/// <returns>Success status if its valid else failure</returns>
		public async Task<IActionResult> CreateOrUpdatePortalToGlobalConfigurationMessages(List<Portal2GlobalConfigurationMessage> portal2GlobalConfigurationMessage)
		{
			try
			{
				if (portal2GlobalConfigurationMessage != null)
				{
					var result = 0;
					using (DbConnection dbConnection = new DbConnection())
					{
						using (var connection = dbConnection.Connection)
						{
							foreach (var item in portal2GlobalConfigurationMessage)
							{
								DynamicParameters parameters = new DynamicParameters();
								parameters.Add("@Id", item.Id);
								parameters.Add("@ValueOverride", item.ValueOverride);
								parameters.Add("@PortalId", item.PortalId);
								parameters.Add("@GlobalConfigurationMessageId", item.GlobalConfigurationMessageId);
								result = connection.Execute("spCreateOrUpdatePortalToGlobalConfigurationMessages", parameters, commandType: CommandType.StoredProcedure);
							}
						}
						if (result != 0)
						{
							return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataSavedSuccessfully, "Portal2GlobalConfiguration Messages") });
						}
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgUpdatingDataError) });
					}
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
