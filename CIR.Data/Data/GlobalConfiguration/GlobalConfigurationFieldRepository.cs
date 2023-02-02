using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using CIR.Core.ViewModel.GlobalConfiguration;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CIR.Data.Data.GlobalConfiguration
{
	public class GlobalConfigurationFieldRepository : IGlobalConfigurationFieldRepository
	{
		#region PROPERTIES
		private readonly CIRDbContext _cIRDbContext;
		#endregion

		#region CONSTRUCTOR
		public GlobalConfigurationFieldRepository(CIRDbContext cIRDbContext)
		{
			_cIRDbContext = cIRDbContext;
		}
		#endregion

		#region METHODS

		/// <summary>
		/// This method gets all fields
		/// </summary>
		/// <param></param>
		/// <returns></returns>
		public async Task<IActionResult> GetAllFields()
		{
			try
			{
				List<GlobalConfigurationField> globalConfigurationField = new();
				using (DbConnection dbConnection = new DbConnection())
				{
					using (var connection = dbConnection.Connection)
					{
						globalConfigurationField = connection.Query<GlobalConfigurationField>("spGetAllFields", null, commandType: CommandType.StoredProcedure).ToList();
					}
				}

				List<GlobalConfigurationFieldViewModel> GlobalConfigurationFieldViewModel = new List<GlobalConfigurationFieldViewModel>();

				foreach (var item in globalConfigurationField)
				{
					GlobalConfigurationFieldViewModel fieldVm = new GlobalConfigurationFieldViewModel();
					fieldVm.FieldName = Enum.GetName(typeof(GlobalConfigurationEnums.Fields), item.FieldTypeId);
					fieldVm.Id = item.Id;
					fieldVm.FieldTypeId = item.FieldTypeId;
					fieldVm.Enabled = item.Enabled;
					fieldVm.Required = item.Required;
					GlobalConfigurationFieldViewModel.Add(fieldVm);
				}

				if (GlobalConfigurationFieldViewModel.Count == 0)
				{
					return new JsonResult(new CustomResponse<List<GlobalConfigurationField>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}
				return new JsonResult(new CustomResponse<List<GlobalConfigurationFieldViewModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = GlobalConfigurationFieldViewModel });

			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method takes a create or update globalconfiguration Fields
		/// </summary>
		/// <param name="globalConfigurationFields"></param>
		/// <returns></returns>

		public async Task<IActionResult> CreateOrUpdateFields(List<GlobalConfigurationField> globalConfigurationFields)
		{
			try
			{
				if (globalConfigurationFields != null)
				{
					var result = 0;
					foreach (var item in globalConfigurationFields)
					{
						using (DbConnection dbConnection = new DbConnection())
						{
							using (var connection = dbConnection.Connection)
							{
								DynamicParameters parameters = new DynamicParameters();
								parameters.Add("@Id", item.Id);
								parameters.Add("@FieldTypeId", item.FieldTypeId);
								parameters.Add("@Enabled", item.Enabled);
								parameters.Add("@Required", item.Required);
								result = connection.Execute("spCreateOrUpdateGlobalConfigurationFields", parameters, commandType: CommandType.StoredProcedure);
							}
						}
					}
					if (result > 0)
					{
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataSavedSuccessfully, "GlobalConfiguration Fields") });
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
