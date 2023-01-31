using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Interfaces.Website;
using CIR.Core.ViewModel.Website;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CIR.Data.Data.Website
{
	public class Portal2GlobalConfigurationCutOffTimesRepository : IPortal2GlobalConfigurationCutOffTimesRepository
	{

		#region PROPERTIES

		private readonly CIRDbContext _CIRDBContext;

		#endregion
		#region CONSTRUCTOR
		public Portal2GlobalConfigurationCutOffTimesRepository(CIRDbContext context)
		{
			_CIRDBContext = context ??
				throw new ArgumentNullException(nameof(context));
		}
		#endregion

		#region METHODS
		/// <summary>
		/// This method used by Portal2GlobalConfigurationCutOffTimes list
		/// </summary>
		/// <param name="portalId"></param>
		/// <returns></returns>
		public async Task<IActionResult> GetPortalToGlobalConfigurationCutOffTimesList(long portalId)
		{
			try
			{
				List<Portal2GlobalConfigurationCutOffTimesModel> portal2GlobalConfigurationCutOffTimesModels = new List<Portal2GlobalConfigurationCutOffTimesModel>();
				using (DbConnection dbConnection = new DbConnection())
				{
					using (var connection = dbConnection.Connection)
					{
						DynamicParameters parameters = new DynamicParameters();
						parameters.Add("portalId", portalId);
						portal2GlobalConfigurationCutOffTimesModels = connection.Query<Portal2GlobalConfigurationCutOffTimesModel>("spGetportal2GlobalConfigurationCutOffTimesPortalWise", parameters, commandType: CommandType.StoredProcedure).ToList();
					}
				}
				if (portal2GlobalConfigurationCutOffTimesModels.Count == 0)
				{
					return new JsonResult(new CustomResponse<List<Portal2GlobalConfigurationCutOffTimesModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}
				return new JsonResult(new CustomResponse<List<Portal2GlobalConfigurationCutOffTimesModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = portal2GlobalConfigurationCutOffTimesModels });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}
		/// <summary>
		/// This method is used by update method of portal2GlobalConfigurationCutOffTimes controller
		/// </summary>
		/// <param name="portal2GlobalConfigurationCutOffTimes"></param>
		/// <returns>Success status if its valid else failure</returns>
		public async Task<IActionResult> UpdatePortalToGlobalConfigurationCutOffTimes(List<Portal2GlobalConfigurationCutOffTimesModel> portal2GlobalConfigurationCutOffTimes)
		{
			try
			{
				if (portal2GlobalConfigurationCutOffTimes != null)
				{
					var result = 0;
					foreach (var item in portal2GlobalConfigurationCutOffTimes)
					{
						using (DbConnection dbConnection = new())
						{
							using (var connection = dbConnection.Connection)
							{
								DynamicParameters parameters = new DynamicParameters();
								parameters.Add("@Id", item.Id);
								parameters.Add("@CutOffDayOverride", item.CutOffDayOverride);
								parameters.Add("@CutOffTimeOverride", TimeSpan.Parse(item.CutOffTimeOverride));
								parameters.Add("@GlobalConfigurationCutOffTimeId", item.GlobalConfigurationCutOffTimeId);
								parameters.Add("@PortalId", item.PortalId);
								result = connection.Execute("spUpdatePortalToGlobalConfigurationCutOffTime", parameters, commandType: CommandType.StoredProcedure);
							}
						}
					}
					if (result != 0)
					{
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataSavedSuccessfully, "Portal2Global CutOffTimes") });
					}
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute() });
				}
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgAddingDataError, "Portal2Global CutOffTimes") });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}
	}
	#endregion
}


