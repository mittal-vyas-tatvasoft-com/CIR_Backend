using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CIR.Data.Data.GlobalConfiguration
{
	public class GlobalConfigurationValidatorRepository : IGlobalConfigurationValidatorRepository
	{
		#region PROPERTIES
		private readonly CIRDbContext _cIRDbContext;
		#endregion
		#region CONSTRUCTOR
		public GlobalConfigurationValidatorRepository(CIRDbContext cIRDbContext)
		{
			_cIRDbContext = cIRDbContext;
		}
		#endregion

		#region METHOD
		/// <summary>
		/// This method returns list of validators
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> GetGlobalConfigurationValidators()
		{
			try
			{
				List<GlobalConfigurationValidator> globalConfigurationValidatorsList = new();
				using (DbConnection dbConnection = new DbConnection())
				{
					using (var connection = dbConnection.Connection)
					{
						globalConfigurationValidatorsList = connection.Query<GlobalConfigurationValidator>("spGetGlobalConfigurationValidator", null, commandType: CommandType.StoredProcedure).ToList();
					}
				}
				if (globalConfigurationValidatorsList.Count == 0)
				{
					return new JsonResult(new CustomResponse<List<GlobalConfigurationReasons>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}
				return new JsonResult(new CustomResponse<List<GlobalConfigurationValidator>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = globalConfigurationValidatorsList });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method takes a create or update globalconfiguration Validators
		/// </summary>
		/// <param name="globalConfigurationValidators"></param>
		/// <returns></returns>
		public async Task<IActionResult> CreateOrUpdateGlobalConfigurationValidators(List<GlobalConfigurationValidator> globalConfigurationValidators)
		{
			try
			{
				if (globalConfigurationValidators != null)
				{
					var result = 0;
					foreach (var item in globalConfigurationValidators)
					{
						using (DbConnection dbConnection = new DbConnection())
						{
							using (var connection = dbConnection.Connection)
							{
								DynamicParameters parameters = new DynamicParameters();
								parameters.Add("@Id", item.Id);
								parameters.Add("@FieldTypeId", item.FieldTypeId);
								parameters.Add("@CultureId", item.CultureId);
								parameters.Add("@Enabled", item.Enabled);
								parameters.Add("@Regex", item.Regex);
								parameters.Add("@ErrorMessage", item.ErrorMessage);
								result = connection.Execute("spCreateOrUpdateGlobalConfigurationValidators", parameters, commandType: CommandType.StoredProcedure);
							}
						}
					}
					if (result != 0)
					{
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataSavedSuccessfully, "GlobalConfiguration Validators") });
					}
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute() });
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
