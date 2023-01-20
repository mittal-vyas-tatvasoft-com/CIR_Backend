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
				Offices newOffice = new()
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
					_CIRDbContext.offices.Update(newOffice);
				}
				else
				{
					_CIRDbContext.offices.Add(newOffice);
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
		/// <param name="sortAscending"> 'asc' or 'desc' direction for sort </param>
		/// <returns> filtered list of users </returns>
		public async Task<IActionResult> GetOffices(int displayLength, int displayStart, string sortCol, string search, bool sortAscending = true)
		{
			OfficeModel office = new();


			if (string.IsNullOrEmpty(sortCol))
			{
				sortCol = "Id";
			}
			try
			{
				office.Count = _CIRDbContext.offices.Where(y => y.Name.Contains(search)).Count();

				var officeRecords = await (from officedata in _CIRDbContext.offices
										   join country in _CIRDbContext.CountryCodes
											on officedata.CountryCode equals country.Id
										   select new officevm()
										   {
											   Id = officedata.Id,
											   Address = officedata.AddressLine1 + officedata.AddressLine2,
											   country = country.CountryName,
											   Name = officedata.Name
										   }).ToListAsync();

				if (officeRecords.Count == 0)
				{
					return new JsonResult(new CustomResponse<List<OfficeModel>>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = null });
				}


				officeRecords = officeRecords.Where(y => y.Name.Contains(search) || y.Address.Contains(search) || y.country.Contains(search)).ToList();
				office.Count = officeRecords.Count();

				if (sortAscending)
				{
					officeRecords = officeRecords.OrderBy(x => x.GetType().GetProperty(sortCol).GetValue(x, null)).Skip(displayStart).Take(displayLength).ToList();
				}
				else
				{
					officeRecords = officeRecords.OrderByDescending(x => x.GetType().GetProperty(sortCol).GetValue(x, null)).Skip(displayStart).Take(displayLength).ToList();
				}
				office.OfficeList = officeRecords;
				return new JsonResult(new CustomResponse<OfficeModel>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = office });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = true, Message = HttpStatusCodesMessages.UnprocessableEntity, Data = ex });
			}
		}

		/// <summary>
		/// fetches offices based on input office id
		/// </summary>
		/// <param name="officeId"></param>
		/// <returns> holiday or null holiday if not found </returns>
		public async Task<IActionResult> GetOfficesById(long officeId)
		{
			try
			{
				var officeList = await _CIRDbContext.offices.Where(x => x.Id == officeId).FirstOrDefaultAsync();
				if (officeList == null)
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound });
				}
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
