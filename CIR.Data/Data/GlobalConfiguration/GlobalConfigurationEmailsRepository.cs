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
	public class GlobalConfigurationEmailsRepository : ControllerBase, IGlobalConfigurationEmailsRepository
	{
		#region PROPERTIES

		private readonly CIRDbContext _CIRDBContext;

		#endregion

		#region CONSTRUCTOR
		public GlobalConfigurationEmailsRepository(CIRDbContext context)
		{
			_CIRDBContext = context ??
				throw new ArgumentNullException(nameof(context));
		}
		#endregion

		#region METHODS
		/// <summary>
		/// This method used by GlobalConfigEmail list
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<IActionResult> GetGlobalConfigurationEmailsDataList(int cultureId)
		{
			try
			{
				List<GlobalConfigurationEmails> globalConfigurationEmailsList;

				using (DbConnection dbConnection = new DbConnection())
				{
					using (var connection = dbConnection.Connection)
					{
						DynamicParameters parameters = new DynamicParameters();
						parameters.Add("@CultureId", cultureId);
						globalConfigurationEmailsList = connection.Query<GlobalConfigurationEmails>("spGetGlobalConfigurationEmailsByCultureId", parameters, commandType: CommandType.StoredProcedure).ToList();
					}
				}

				if (globalConfigurationEmailsList.Count == 0)
				{
					return new JsonResult(new CustomResponse<List<GlobalConfigurationEmails>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}
				return new JsonResult(new CustomResponse<List<GlobalConfigurationEmails>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = globalConfigurationEmailsList });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}
		/// <summary>
		/// This method is used by create method and update method of globalMessage controller
		/// </summary>
		/// <param name="globalConfigurationEmails"></param>
		/// <returns>Success status if its valid else failure</returns>
		public async Task<IActionResult> CreateOrUpdateGlobalConfigurationEmails(List<GlobalConfigurationEmails> globalConfigurationEmails)
		{
			try
			{

				if (globalConfigurationEmails.Any(x => x.Id == 0))
				{
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute() });

				}

				string? result = "";
				using (DbConnection dbConnection = new DbConnection())
				{

					using (var connection = dbConnection.Connection)
					{
						foreach (var email in globalConfigurationEmails)
						{
							DynamicParameters parameters = new DynamicParameters();
							parameters.Add("@Id", email.Id);
							parameters.Add("@FieldTypeId", email.FieldTypeId);
							parameters.Add("@CultureId", email.CultureId);
							parameters.Add("@Content", email.Content);
							parameters.Add("@Subject", email.Subject);

							result = Convert.ToString(connection.ExecuteScalar("spUpdateGlobalConfigurationEmails", parameters, commandType: CommandType.StoredProcedure));
						}
					}
				}
				if (result != "False")
				{
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataUpdatedSuccessfully, "GlobalConfiguration Emails") });
				}
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}
		#endregion
	}
}

