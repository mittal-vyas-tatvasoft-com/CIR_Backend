using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
		public async Task<IActionResult> GetGlobalConfigurationEmailsDataList(int id)
		{
			try
			{
				var globalConfigEmails = (from globalMessages in _CIRDBContext.GlobalConfigurationEmails
										  select new GlobalConfigurationEmailsGetModel()
										  {
											  Id = globalMessages.Id,
											  CultureId = globalMessages.CultureId,
											  FieldTypeId = globalMessages.FieldTypeId,
											  Content = globalMessages.Content,
											  Subject = globalMessages.Subject,

										  }).Where(x => x.CultureId == id).ToList();

				if (globalConfigEmails.Count == 0)
				{
					return new JsonResult(new CustomResponse<List<GlobalConfigurationEmailsGetModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}

				var emailId = await _CIRDBContext.GlobalConfigurationEmails.Where(x => x.CultureId == id).FirstOrDefaultAsync();
				var serializedEmailId = JsonConvert.SerializeObject(emailId);
				GlobalConfigurationEmailsGetModel email = JsonConvert.DeserializeObject<GlobalConfigurationEmailsGetModel>(serializedEmailId);

				if (email.Content.Contains("[Reference])") || email.Subject.Contains("[Reference])"))
				{
					email.Reference = true;
				}
				if (email.Content.Contains("[BookingId])") || email.Subject.Contains("[BookingId])"))
				{
					email.BookingId = true;
				}
				if (email.Content.Contains("[OrderNumber])") || email.Subject.Contains("[OrderNumber])"))
				{
					email.OrderNumber = true;
				}
				if (email.Content.Contains("[CustomerEmail])") || email.Subject.Contains("[CustomerEmail])"))
				{
					email.CustomerEmail = true;
				}
				if (email.Content.Contains("[CustomerName])") || email.Subject.Contains("[CustomerName])"))
				{
					email.CustomerName = true;
				}
				if (email.Content.Contains("[CollectionAddress])") || email.Subject.Contains("[CollectionAddress])"))
				{
					email.CollectionAddress = true;
				}
				if (email.Content.Contains("[TrackingURL])") || email.Subject.Contains("[TrackingURL])"))
				{
					email.TrackingURL = true;
				}
				if (email.Content.Contains("[LabelURL])") || email.Subject.Contains("[LabelURL])"))
				{
					email.LabelURL = true;
				}
				if (email.Content.Contains("[BookingURL])") || email.Subject.Contains("[BookingURL])"))
				{
					email.BookingURL = true;
				}
				if (globalConfigEmails != null)
					return new JsonResult(new CustomResponse<List<GlobalConfigurationEmailsGetModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = globalConfigEmails });
				else
					return new JsonResult(new CustomResponse<List<GlobalConfigurationEmailsGetModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });

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
				if (globalConfigurationEmails != null)
				{
					foreach (var item in globalConfigurationEmails)
					{
						if (item.Id != 0)
						{
							var emailsCheck = _CIRDBContext.GlobalConfigurationEmails.FirstOrDefault(x => x.Id == item.Id);
							if (emailsCheck != null)
							{
								emailsCheck.FieldTypeId = item.FieldTypeId;
								emailsCheck.CultureId = item.CultureId;
								emailsCheck.Content = item.Content;
								emailsCheck.Subject = item.Subject;
							}
							else
							{
								return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute() });
							}
						}
						else
						{
							GlobalConfigurationEmails email = new GlobalConfigurationEmails()
							{
								Id = item.Id,
								FieldTypeId = item.FieldTypeId,
								CultureId = item.CultureId,
								Content = item.Content,
								Subject = item.Subject
							};
							_CIRDBContext.GlobalConfigurationEmails.Add(email);
						}
					}
					await _CIRDBContext.SaveChangesAsync();
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataSavedSuccessfully, "GlobalConfiguration Email") });
				}
				return new JsonResult(new CustomResponse<List<GlobalConfigurationEmails>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}
		#endregion
	}
}

