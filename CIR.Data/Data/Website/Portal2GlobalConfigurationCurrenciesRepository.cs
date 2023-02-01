using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities.Websites;
using CIR.Core.Interfaces.Website;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CIR.Data.Data.Website
{
	public class Portal2GlobalConfigurationCurrenciesRepository : IPortal2GlobalConfigurationCurrenciesRepository
	{
		#region PROPERTIES

		private readonly CIRDbContext _CIRDBContext;

		#endregion
		#region CONSTRUCTOR
		public Portal2GlobalConfigurationCurrenciesRepository(CIRDbContext context)
		{
			_CIRDBContext = context ??
				throw new ArgumentNullException(nameof(context));
		}
		#endregion
		#region METHODS
		/// <summary>
		/// This method used by Portal2GlobalConfigurationCurrencies list
		/// </summary>
		/// <param name="portalId"></param>
		/// <returns></returns>
		public async Task<IActionResult> GetPortalToGlobalConfigurationCurrenciesList(long portalId)
		{
			try
			{
				List<Portal2GlobalConfigurationCurrency> portal2GlobalConfigurationCurrencies;

				using (DbConnection dbConnection = new DbConnection())
				{

					using (var connection = dbConnection.Connection)
					{
						DynamicParameters parameters = new DynamicParameters();
						parameters.Add("@portalId", portalId);
						portal2GlobalConfigurationCurrencies = connection.Query<Portal2GlobalConfigurationCurrency>("spGetportal2GlobalConfigurationCurrenciesPortalWise", parameters, commandType: CommandType.StoredProcedure).ToList();
					}
				}
				if (portal2GlobalConfigurationCurrencies.Count == 0)
				{
					return new JsonResult(new CustomResponse<List<Portal2GlobalConfigurationCurrency>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}
				return new JsonResult(new CustomResponse<List<Portal2GlobalConfigurationCurrency>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = portal2GlobalConfigurationCurrencies });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}

		}


		/// <summary>
		/// This method is used by create method and update method of portal2GlobalConfigurationCurrencies controller
		/// </summary>
		/// <param name="portal2GlobalConfigurationCurrencies"></param>
		/// <returns>Success status if its valid else failure</returns>
		public async Task<IActionResult> UpdatePortalToGlobalConfigurationCurrencies(List<Portal2GlobalConfigurationCurrency> portal2GlobalConfigurationCurrencies)
		{
			try
			{
				if (portal2GlobalConfigurationCurrencies != null)
				{
					var result = 0;
					foreach (var item in portal2GlobalConfigurationCurrencies)
					{
						using (DbConnection dbConnection = new())
						{
							using (var connection = dbConnection.Connection)
							{
								DynamicParameters parameters = new DynamicParameters();
								parameters.Add("@Id", item.Id);
								parameters.Add("@PortalId", item.PortalId);
								parameters.Add("@EnabledOverride", item.EnabledOverride);
								parameters.Add("@GlobalConfigurationCurrencyId", item.GlobalConfigurationCurrencyId);
								result = connection.Execute("spUpdatePortalToGlobalConfigurationCurrencies", parameters, commandType: CommandType.StoredProcedure);
							}
						}
					}
					if (result != 0)
					{
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataUpdatedSuccessfully, "Portal2Global Currency") });
					}
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute() });
				}
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgUpdatingDataError, "Portal2Global Currency") });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}
		#endregion
	}
}

