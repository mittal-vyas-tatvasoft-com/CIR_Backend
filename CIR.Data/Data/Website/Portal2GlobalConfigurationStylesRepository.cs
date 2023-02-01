using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities;
using CIR.Core.Interfaces.Website;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CIR.Data.Data.Website
{
	public class Portal2GlobalConfigurationStylesRepository : ControllerBase, IPortal2GlobalConfigurationStylesRepository
	{
		#region PROPERTIES
		private readonly CIRDbContext _CIRDbContext;
		#endregion

		#region CONSTRUCTOR
		public Portal2GlobalConfigurationStylesRepository(CIRDbContext context)
		{
			_CIRDbContext = context ??
				throw new ArgumentNullException(nameof(context));
		}
		#endregion

		#region METHODS
		/// <summary>
		/// This method used by Portal2GlobalConfigurationStylesList
		/// </summary>
		/// <param name="portalId"></param>
		/// <returns></returns>
		public async Task<IActionResult> GetPortalToGlobalConfigurationStylesList(long portalId)
		{
			try
			{
				List<Portal2GlobalConfigurationStyle> portal2GlobalConfigurationStyles;

				using (DbConnection dbConnection = new DbConnection())
				{

					using (var connection = dbConnection.Connection)
					{
						DynamicParameters parameters = new DynamicParameters();
						parameters.Add("@PortalId", portalId);
						portal2GlobalConfigurationStyles = connection.Query<Portal2GlobalConfigurationStyle>("spGetportal2GlobalConfigurationStylesPortalWise", parameters, commandType: CommandType.StoredProcedure).ToList();
					}
				}
				if (portal2GlobalConfigurationStyles.Count == 0)
				{
					return new JsonResult(new CustomResponse<List<Portal2GlobalConfigurationStyle>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}
				return new JsonResult(new CustomResponse<List<Portal2GlobalConfigurationStyle>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = portal2GlobalConfigurationStyles });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}

		}

		/// <summary>
		/// This method is used update method of portal2GlobalConfigurationStyle controller
		/// </summary>
		/// <param name="portal2GlobalConfigurationStyles"></param>
		/// <returns>Success status if its valid else failure</returns>
		public async Task<IActionResult> UpdatePortalToGlobalConfigurationStyles(List<Portal2GlobalConfigurationStyle> portal2GlobalConfigurationStyles)
		{
			try
			{
				if (portal2GlobalConfigurationStyles != null)
				{
					var result = 0;
					foreach (var item in portal2GlobalConfigurationStyles)
					{
						using (DbConnection dbConnection = new())
						{
							using (var connection = dbConnection.Connection)
							{
								DynamicParameters parameters = new DynamicParameters();
								parameters.Add("@Id", item.Id);
								parameters.Add("@PortalId", item.PortalId);
								parameters.Add("@GlobalConfigurationStyleId", item.GlobalConfigurationStyleId);
								parameters.Add("@ValueOverride", item.ValueOverride);
								result = connection.Execute("spUpdatePortalToGlobalConfigurationStyles", parameters, commandType: CommandType.StoredProcedure);
							}
						}
					}
					if (result != 0)
					{
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataUpdatedSuccessfully, "Portal To Global Configration Styles") });
					}
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute() });
				}
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgUpdatingDataError, "Portal To Global Configration Styles") });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}
		#endregion
	}
}
