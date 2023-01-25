using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Entities.Website;
using CIR.Core.Entities.Websites;
using CIR.Core.Interfaces.Website;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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
				var result = (from PGCurrency in _CIRDBContext.Portal2GlobalConfigurationCurrencies
							  select new Portal2GlobalConfigurationCurrency()
							  {
								  Id = PGCurrency.Id,
								  PortalId = PGCurrency.PortalId,
								  GlobalConfigurationCurrencyId = PGCurrency.GlobalConfigurationCurrencyId,
								  EnabledOverride = PGCurrency.EnabledOverride,


							  }).Where(x =>

							  x.PortalId == portalId).ToList();


				var CurrencyId = (from PGCC in _CIRDBContext.Portal2GlobalConfigurationCurrencies
								  select new Portal2GlobalConfigurationCurrency()
								  {
									  Id = PGCC.Id,
									  PortalId = PGCC.PortalId,
									  GlobalConfigurationCurrencyId = PGCC.GlobalConfigurationCurrencyId,
									  EnabledOverride = PGCC.EnabledOverride
								  }).FirstOrDefault(x => x.PortalId == portalId);
				var serializedParent = JsonConvert.SerializeObject(CurrencyId);
				Portal2GlobalConfigurationCurrency Currency = JsonConvert.DeserializeObject<Portal2GlobalConfigurationCurrency>(serializedParent);

				if (Currency.EnabledOverride)
				{
					Currency.EnabledOverride = true;
				}

				if (result != null)
				{
					return new JsonResult(new CustomResponse<List<Portal2GlobalConfigurationCurrency>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = result });
				}
				else
				{
					return new JsonResult(new CustomResponse<List<Portal2GlobalConfigurationCurrency>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}
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
					foreach (var item in portal2GlobalConfigurationCurrencies)
					{
						var Currency = _CIRDBContext.Portal2GlobalConfigurationCurrencies.FirstOrDefault(x => x.Id == item.Id);
						if (Currency != null)
						{
							Currency.PortalId = item.PortalId;
							Currency.GlobalConfigurationCurrencyId = item.GlobalConfigurationCurrencyId;
							Currency.EnabledOverride = item.EnabledOverride;
						}
						else
						{
							return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute() });
						}

						_CIRDBContext.Portal2GlobalConfigurationCurrencies.Update(Currency);
					}
					await _CIRDBContext.SaveChangesAsync();
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataUpdatedSuccessfully, "Portal To Global Configuration Currencies") });
				}
				return new JsonResult(new CustomResponse<PortalToGlobalConfigurationEmails>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}
		#endregion
	}
}
