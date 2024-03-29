﻿using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities.Website;
using CIR.Core.Interfaces.Website;
using CIR.Core.ViewModel.Website;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;

namespace CIR.Data.Data.Website
{
	public class PortalToGlobalConfigurationEmailsRepository : ControllerBase, IPortalToGlobalConfigurationEmailsRepository
	{
		#region PROPERTIES

		private readonly CIRDbContext _CIRDBContext;

		#endregion
		#region CONSTRUCTOR
		public PortalToGlobalConfigurationEmailsRepository(CIRDbContext context)
		{
			_CIRDBContext = context ??
				throw new ArgumentNullException(nameof(context));
		}
		#endregion
		#region METHODS
		/// <summary>
		/// This method used by PortalToGlobalConfigurationEmails list
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<IActionResult> GetPortalToGlobalConfigurationEmailsList(int id)
		{
			try
			{
				List<PortalToGlobalConfigurationEmailsGetModel> portalToGlobalConfigurationEmailsGetModel = new();
				using (DbConnection dbConnection = new DbConnection())
				{
					using (var connection = dbConnection.Connection)
					{
						DynamicParameters parameters = new DynamicParameters();
						parameters.Add("@id", id);
						portalToGlobalConfigurationEmailsGetModel = connection.Query<PortalToGlobalConfigurationEmailsGetModel>("spGetEmailIdById", parameters, commandType: CommandType.StoredProcedure).ToList();
					}
				}
				if (portalToGlobalConfigurationEmailsGetModel.Count == 0)
				{
					return new JsonResult(new CustomResponse<List<PortalToGlobalConfigurationEmailsGetModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}


				var emailId = await _CIRDBContext.Portal2GlobalConfigurationEmails.Where(x => x.Id == id).FirstOrDefaultAsync();
				var serializedEmail = JsonConvert.SerializeObject(emailId);
				PortalToGlobalConfigurationEmailsGetModel email = JsonConvert.DeserializeObject<PortalToGlobalConfigurationEmailsGetModel>(serializedEmail);

				if (email.ContentOverride.Contains("[Reference]") || email.SubjectOverride.Contains("[Reference]"))
				{
					email.Reference = true;
				}
				if (email.ContentOverride.Contains("[BookingId]") || email.SubjectOverride.Contains("[BookingId]"))
				{
					email.BookingId = true;
				}
				if (email.ContentOverride.Contains("[OrderNumber]") || email.SubjectOverride.Contains("[OrderNumber]"))
				{
					email.OrderNumber = true;
				}
				if (email.ContentOverride.Contains("[CollectionDate]") || email.SubjectOverride.Contains("[CollectionDate]"))
				{
					email.CollectionDate = true;
				}
				if (email.ContentOverride.Contains("[CustomerEmail]") || email.SubjectOverride.Contains("[CustomerEmail]"))
				{
					email.CustomerEmail = true;
				}
				if (email.ContentOverride.Contains("[CustomerName]") || email.SubjectOverride.Contains("[CustomerName]"))
				{
					email.CustomerName = true;
				}
				if (email.ContentOverride.Contains("[CollectionAddress]") || email.SubjectOverride.Contains("[CollectionAddress]"))
				{
					email.CollectionAddress = true;
				}
				if (email.ContentOverride.Contains("[TrackingURL]") || email.SubjectOverride.Contains("[TrackingURL]"))
				{
					email.TrackingURL = true;
				}
				if (email.ContentOverride.Contains("[LabelURL]") || email.SubjectOverride.Contains("[LabelURL]"))
				{
					email.LabelURL = true;
				}
				if (email.ContentOverride.Contains("[BookingURL]") || email.SubjectOverride.Contains("[BookingURL]"))
				{
					email.BookingURL = true;
				}
				if (portalToGlobalConfigurationEmailsGetModel != null)
					return new JsonResult(new CustomResponse<List<PortalToGlobalConfigurationEmailsGetModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = portalToGlobalConfigurationEmailsGetModel });
				else
					return new JsonResult(new CustomResponse<List<PortalToGlobalConfigurationEmailsGetModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });

			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}
		/// <summary>
		/// This method is used by create method and update method of portalToGlobalConfigurationEmails controller
		/// </summary>
		/// <param name="portalToGlobalConfigurationEmails"></param>
		/// <returns>Success status if its valid else failure</returns>
		public async Task<IActionResult> CreateOrUpdatePortalToGlobalConfigurationEmails(List<PortalToGlobalConfigurationEmails> portalToGlobalConfigurationEmails)
		{
			try
			{
				if (portalToGlobalConfigurationEmails != null)
				{
					var result = 0;
					using (DbConnection dbConnection = new DbConnection())
					{
						using (var connection = dbConnection.Connection)
						{
							foreach (var item in portalToGlobalConfigurationEmails)
							{
								DynamicParameters parameters = new DynamicParameters();
								parameters.Add("@Id", item.Id);
								parameters.Add("@PortalId", item.PortalId);
								parameters.Add("@GlobalConfigurationEmailId", item.GlobalConfigurationEmailId);
								parameters.Add("@ContentOverride", item.ContentOverride);
								parameters.Add("@SubjectOverride", item.SubjectOverride);
								result = connection.Execute("spCreateUpdatePortaltoGlobalConfigEmails", parameters, commandType: CommandType.StoredProcedure);
							}
						}
						if (result != 0)
						{
							return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataSavedSuccessfully, "Portal2GlobalConfiguration Emails") });
						}
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute(), Data = SystemMessages.msgSomethingWentWrong });
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
