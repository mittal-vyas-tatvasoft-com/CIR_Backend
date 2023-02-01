using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Interfaces.Websites;
using CIR.Core.ViewModel.Websites;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using DbConnection = CIR.Common.Data.DbConnection;

namespace CIR.Data.Data.Websites
{
	public class Portal2GlobalConfigurationReasonsRepository : IPortal2GlobalConfigurationReasonsRepository
	{

		#region PROPERTIES
		private readonly CIRDbContext _CIRDbContext;
		#endregion

		#region CONSTRUCTOR
		public Portal2GlobalConfigurationReasonsRepository(CIRDbContext CIRDbContext)
		{
			_CIRDbContext = CIRDbContext ??
				throw new ArgumentNullException(nameof(CIRDbContext));
		}
		#endregion

		#region METHODS

		/// <summary>
		/// This method will be used by create method of Reasons controller
		/// </summary>
		/// <param name="reasonsModel"></param>
		/// <returns>return Ok if successful else returns bad request</returns>

		public async Task<IActionResult> CreateReason(List<Portal2GlobalConfigurationReasonsModel> reasonsModel)
		{
			try
			{
				var result = 0;
				//offices 
				using (DbConnection dbConnection = new DbConnection())
				{
					using (var connection = dbConnection.Connection)
					{
						foreach (var item in reasonsModel)
						{
							DynamicParameters parameters = new DynamicParameters();
							parameters.Add("@AddressLine1", item.AddressLine1);
							parameters.Add("@TownCity", item.TownCity);
							parameters.Add("@CountryCode", item.CountryCode);
							parameters.Add("@CreatedOn", item.CreatedOn);
							parameters.Add("@Enabled", item.Enabled);
							parameters.Add("@Latitude", item.Latitude);
							parameters.Add("@Longitude", item.Longitude);
							parameters.Add("@AddressType", item.AddressType);
							result = connection.Execute("spPToGReasonsCreateOffice", parameters, commandType: CommandType.StoredProcedure);
						}
					}
					if (result == 0)
					{
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute(), Data = SystemMessages.msgSomethingWentWrong });
					}
				}
				var officeId = _CIRDbContext.offices.OrderByDescending(x => x.Id).FirstOrDefault().Id;

				//Portals
				using (DbConnection dbConnection = new DbConnection())
				{
					using (var connection = dbConnection.Connection)
					{
						foreach (var item in reasonsModel)
						{
							DynamicParameters parameters = new DynamicParameters();
							parameters.Add("@ClientId", item.ClientId);
							parameters.Add("@CurrencyId", item.CurrencyId);
							parameters.Add("@CountryId", item.CountryId);
							parameters.Add("@CultureId", item.CultureId);
							parameters.Add("@IntegrationLevel", item.IntegrationLevel);
							parameters.Add("@ReturnItemsEnabled", item.ReturnItemsEnabled);
							parameters.Add("@CreateResponse", item.CreateResponse);
							parameters.Add("@CountReturnIdentifier", item.CountReturnIdentifier);
							result = connection.Execute("spPToGReasonsCreatePortal", parameters, commandType: CommandType.StoredProcedure);
						}
					}
					if (result == 0)
					{
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute(), Data = SystemMessages.msgSomethingWentWrong });
					}
				}
				var portalId = _CIRDbContext.portals.OrderByDescending(x => x.Id).FirstOrDefault().Id;

				//GlobalReasons
				using (DbConnection dbConnection = new DbConnection())
				{
					using (var connection = dbConnection.Connection)
					{
						foreach (var item in reasonsModel)
						{
							DynamicParameters parameters = new DynamicParameters();
							parameters.Add("@Content", item.Content);
							parameters.Add("@Enabled", item.Enabled);
							parameters.Add("@Type", item.Type);
							result = connection.Execute("spPToGReasonsCreateGlobalConfigurationReasons", parameters, commandType: CommandType.StoredProcedure);
						}
					}
					if (result == 0)
					{
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute(), Data = SystemMessages.msgSomethingWentWrong });
					}
				}
				var globalReasonId = _CIRDbContext.GlobalConfigurationReasons.OrderByDescending(x => x.Id).FirstOrDefault().Id;

				//Portal2GlobalConfigurationReasons
				using (DbConnection dbConnection = new DbConnection())
				{
					using (var connection = dbConnection.Connection)
					{
						foreach (var item in reasonsModel)
						{
							DynamicParameters parameters = new DynamicParameters();
							parameters.Add("@Content", item.Content);
							parameters.Add("@Enabled", item.Enabled);
							parameters.Add("@Type", item.Type);
							result = connection.Execute("spPToGReasonsCreateGlobalConfigurationReasons", parameters, commandType: CommandType.StoredProcedure);
						}
					}
					if (result == 0)
					{
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute(), Data = SystemMessages.msgSomethingWentWrong });
					}
				}
				//Portal2GlobalConfigurationReasons
				using (DbConnection dbConnection = new DbConnection())
				{
					using (var connection = dbConnection.Connection)
					{
						foreach (var item in reasonsModel)
						{
							DynamicParameters parameters = new DynamicParameters();
							parameters.Add("@contentOverride", item.ContentOverride);
							parameters.Add("@officeId", officeId);
							parameters.Add("@countReturnIdentifier", item.Enabled);
							parameters.Add("@globalReasonId", globalReasonId);
							parameters.Add("@portalId", portalId);
							result = connection.Execute("spPToGReasonsCreatePortal2GlobalConfigurationReasons", parameters, commandType: CommandType.StoredProcedure);
						}
					}
					if (result == 0)
					{
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute(), Data = SystemMessages.msgSomethingWentWrong });
					}
				}
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataSavedSuccessfully, "Global Configuration Messages") });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}

		}

		/// <summary>
		/// this method will be used by GetAll method of Reasons controller
		/// </summary>
		/// <returns>returns list of all the Reasons</returns>
		public async Task<IActionResult> GetAllReasons()
		{
			try
			{
				List<Portal2GlobalConfigurationReasonsModel> portal2GlobalConfigurationReasonsModel = new();

				using (DbConnection dbConnection = new DbConnection())
				{
					using (var connection = dbConnection.Connection)
					{
						portal2GlobalConfigurationReasonsModel = connection.Query<Portal2GlobalConfigurationReasonsModel>("spGetAllPortalToGlobalConfigReasons", null, commandType: CommandType.StoredProcedure).ToList();
					}
				}

				if (portal2GlobalConfigurationReasonsModel.Count == 0)
				{
					return new JsonResult(new CustomResponse<List<Portal2GlobalConfigurationReasonsModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}
				return new JsonResult(new CustomResponse<List<Portal2GlobalConfigurationReasonsModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = portal2GlobalConfigurationReasonsModel });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}
		#endregion
	}
}
