using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Entities.Websites;
using CIR.Core.Interfaces.Website;
using CIR.Core.ViewModel.Website;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CIR.Core.ViewModel.Website.OfficeModel;

namespace CIR.Data.Data.Website
{
	public class OfficesRepository : ControllerBase, IOfficesRepository
	{
		#region PROPERTIES
		private readonly CIRDbContext _CIRDbContext;
		#endregion

		#region CONSTRUCTOR
		public OfficesRepository(CIRDbContext CIRDbContext)
		{
			_CIRDbContext = CIRDbContext ??
				throw new ArgumentNullException(nameof(CIRDbContext)); ;
		}
		#endregion

		#region METHODS
		/// <summary>
		/// This method is used by create method and update method of office controller
		/// </summary>
		/// <param name="offices"> new office data or update data for office</param>
		/// <returns> Ok status if its valid else unprocessable </returns>
		public async Task<IActionResult> CreateOrUpdateOffice(Offices offices)
		{
			try
			{
				Offices newoffice = new()
				{
					AddressLine1 = offices.AddressLine1,
					AddressLine2 = offices.AddressLine2,
					AddressType = offices.AddressType,
					AssetId = offices.AssetId,
					CountryCode = offices.CountryCode,
					CreatedOn = offices.CreatedOn,
					Description = offices.Description,
					Email = offices.Email,
					Enabled = offices.Enabled,
					FaxNo = offices.FaxNo,
					Id = offices.Id,
					IsDefault = offices.IsDefault,
					LastEditedOn = offices.LastEditedOn,
					Latitude = offices.Latitude,
					Longitude = offices.Longitude,
					Name = offices.Name,
					Postcode = offices.Postcode,
					RegisteredNo = offices.RegisteredNo,
					StateCounty = offices.StateCounty,
					StoreId = offices.StoreId,
					TelNo = offices.TelNo,
					TownCity = offices.TownCity,
					Website = offices.Website
				};

				if (offices.Id > 0)
				{
					_CIRDbContext.offices.Update(newoffice);
				}
				else
				{
					_CIRDbContext.offices.Add(newoffice);
				}
				await _CIRDbContext.SaveChangesAsync();
				return Ok(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated, Data = "Offices saved successfully." });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
			}
		}

		/// <summary>
		/// This method retuns filtered holidays list
		/// </summary>
		/// <param name="displayLength"> how many row/data we want to fetch(for pagination) </param>
		/// <param name="displayStart"> from which row we want to fetch(for pagination) </param>
		/// <param name="sortCol"> name of column which we want to sort</param>
		/// <param name="search"> word that we want to search in user table </param>
		/// <param name="sortDir"> 'asc' or 'desc' direction for sort </param>
		/// <returns> filtered list of users </returns>
		public async Task<IActionResult> GetHolidays(int displayLength, int displayStart, string sortCol, string search, bool sortAscending = true)
		{
			OfficeModel Office = new();


			if (string.IsNullOrEmpty(sortCol))
			{
				sortCol = "Id";
			}
			try
			{
				Office.Count = _CIRDbContext.offices.Where(y => y.Name.Contains(search)).Count();

				var sortData = await (from officedata in _CIRDbContext.offices
									  join country in _CIRDbContext.CountryCodes
									   on officedata.CountryCode equals country.Id
									  select new officevm()
									  {
										  Id = officedata.Id,
										  Address = officedata.AddressLine1 + officedata.AddressLine2,
										  country = country.CountryName,
										  Name = officedata.Name
									  }).ToListAsync();


				sortData = sortData.Where(y => y.Name.Contains(search) || y.Address.Contains(search) || y.country.Contains(search)).ToList();
				Office.Count = sortData.Count();

				if (sortAscending)
				{
					sortData = sortData.OrderBy(x => x.GetType().GetProperty(sortCol).GetValue(x, null)).Skip(displayStart).Take(displayLength).ToList();
				}
				else
				{
					sortData = sortData.OrderByDescending(x => x.GetType().GetProperty(sortCol).GetValue(x, null)).Skip(displayStart).Take(displayLength).ToList();
				}
				Office.OfficeList = sortData;
				return new JsonResult(new CustomResponse<OfficeModel>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = Office });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = true, Message = HttpStatusCodesMessages.UnprocessableEntity, Data = ex });
			}
		}

		/// <summary>
		/// fetches offices based on input office id
		/// </summary>
		/// <param name="id"></param>
		/// <returns> holiday or null holiday if not found </returns>
		public async Task<IActionResult> GetOfficesById(long id)
		{
			try
			{
				var officeList = await _CIRDbContext.offices.Where(x => x.Id == id).FirstOrDefaultAsync();
				return new JsonResult(new CustomResponse<Offices>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = officeList });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
			}
		}


		/// <summary>
		/// This method takes a delete holiday 
		/// </summary>
		/// <param name="officeId"></param>
		/// <returns></returns>
		public async Task<IActionResult> DeleteOffice(long officeId)
		{
			var office = new Holidays() { Id = officeId };
			try
			{
				_CIRDbContext.offices.RemoveRange(_CIRDbContext.offices.Where(x => x.Id == officeId));
				await _CIRDbContext.SaveChangesAsync();
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Deleted, Data = "office Deleted Successfully" });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = true, Message = HttpStatusCodesMessages.UnprocessableEntity, Data = ex });
			}
		}
		#endregion
	}
}
