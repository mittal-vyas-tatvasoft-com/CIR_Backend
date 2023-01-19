using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Entities.Websites;
using CIR.Core.Interfaces.Websites;
using CIR.Core.ViewModel.Websites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
		/// <param name="ReasonsModel"></param>
		/// <returns>return Ok if successful else returns bad request</returns>

		public async Task<IActionResult> CreateReason(List<Portal2GlobalConfigurationReasonsModel> ReasonsModel)
		{
			try
			{
				if (ReasonsModel.Any(x => x.Id < 0 || x.OficeIdPK < 0 || x.PortalIdPK < 0))
				{
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Error occurred while adding new Reasons" });
				}
				if (ReasonsModel != null)
				{
					foreach (var item in ReasonsModel)
					{
						var newOffice = new Offices()
						{
							AddressLine1 = _CIRDbContext.offices.FirstOrDefault(x => x.IsDefault == true).AddressLine1,
							TownCity = _CIRDbContext.offices.FirstOrDefault(x => x.IsDefault == true).TownCity,
							CountryCode = _CIRDbContext.offices.FirstOrDefault(x => x.IsDefault == true).CountryCode,
							CreatedOn = _CIRDbContext.offices.FirstOrDefault(x => x.IsDefault == true).CreatedOn,
							Enabled = _CIRDbContext.offices.FirstOrDefault(x => x.IsDefault == true).Enabled,
							Latitude = _CIRDbContext.offices.FirstOrDefault(x => x.IsDefault == true).Latitude,
							Longitude = _CIRDbContext.offices.FirstOrDefault(x => x.IsDefault == true).Longitude,
							AddressType = _CIRDbContext.offices.FirstOrDefault(x => x.IsDefault == true).AddressType
						};
						_CIRDbContext.offices.Add(newOffice);
						_CIRDbContext.SaveChanges();

						var officeId = newOffice.Id;

						var newPortal = new Portals()
						{
							ClientId = item.ClientId,
							CurrencyId = item.CurrencyId,
							CountryId = item.CountryId,
							CultureId = item.CultureId,
							IntegrationLevel = item.IntegrationLevel,
							ReturnItemsEnabled = item.ReturnItemsEnabled,
							CreateResponse = item.CreateResponse,
							CountReturnIdentifier = item.CountReturnIdentifier
						};
						_CIRDbContext.portals.Add(newPortal);
						_CIRDbContext.SaveChanges();

						var portalId = newPortal.Id;

						var newGlobalConfigReasons = new GlobalConfigurationReasons()
						{
							Content = item.Content,
							Enabled = item.globalconfidEnabled,
							Type = item.Type
						};
						_CIRDbContext.GlobalConfigurationReasons.Add(newGlobalConfigReasons);
						_CIRDbContext.SaveChanges();

						var globalConfigReasonId = newGlobalConfigReasons.Id;

						var globalConfigurationReasons = new Portal2GlobalConfigurationReasons()
						{
							ContentOverride = item.ContentOverride,
							DestinationId = officeId,
							Enabled = item.Enabled,
							PortalId = portalId,
							GlobalConfigurationReasonId = globalConfigReasonId
						};
						_CIRDbContext.portal2GlobalConfigurationReasons.Add(globalConfigurationReasons);
						_CIRDbContext.SaveChanges();

						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = "Reason Added Successfully" });
					}
				}
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Error occurred while adding new Reason" });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
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
				var reasons = await (from Portal2GlobalConfigurationReason in _CIRDbContext.portal2GlobalConfigurationReasons
									 join Office in _CIRDbContext.offices
									 on Portal2GlobalConfigurationReason.DestinationId equals Office.Id
									 join Portal in _CIRDbContext.portals
									 on Portal2GlobalConfigurationReason.PortalId equals Portal.Id
									 join GlobalConfigurationReasons in _CIRDbContext.GlobalConfigurationReasons
									 on Portal2GlobalConfigurationReason.GlobalConfigurationReasonId equals GlobalConfigurationReasons.Id

									 select new Portal2GlobalConfigurationReasonsModel()
									 {
										 Id = Portal2GlobalConfigurationReason.Id,
										 ContentOverride = Portal2GlobalConfigurationReason.ContentOverride,
										 DestinationId = Portal2GlobalConfigurationReason.DestinationId,
										 Enabled = Portal2GlobalConfigurationReason.Enabled,
										 GlobalConfigurationReasonId = Portal2GlobalConfigurationReason.GlobalConfigurationReasonId,
										 PortalId = Portal2GlobalConfigurationReason.PortalId
									 }).ToListAsync();

				if (reasons.Count == 0)
				{
					return new JsonResult(new CustomResponse<List<Portal2GlobalConfigurationReasonsModel>>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = null });
				}

				return new JsonResult(new CustomResponse<List<Portal2GlobalConfigurationReasonsModel>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = reasons });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
			}
		}
		#endregion
	}
}
