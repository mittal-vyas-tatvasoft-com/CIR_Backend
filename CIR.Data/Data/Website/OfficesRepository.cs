using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities.Websites;
using CIR.Core.Interfaces.Website;
using CIR.Core.ViewModel.Website;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
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
        /// <param name="office"> new office data or update data for office</param>
        /// <returns> Ok status if its valid else unprocessable </returns>
        public async Task<IActionResult> CreateOrUpdateOffice(Offices office)
        {
            try
            {
                var result = 0;
                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@Id", office.Id);
                        parameters.Add("@Name", office.Name);
                        parameters.Add("@AddressLine1", office.AddressLine1);
                        parameters.Add("@AddressLine2", office.AddressLine2);
                        parameters.Add("@TownCity", office.TownCity);
                        parameters.Add("@StateCountry", office.StateCounty);
                        parameters.Add("@CountryCode", office.CountryCode);
                        parameters.Add("@PostCode", office.Postcode);
                        parameters.Add("@TelNo", office.TelNo);
                        parameters.Add("@FaxNo", office.FaxNo);
                        parameters.Add("@Email", office.Email);
                        parameters.Add("@Website", office.Website);
                        parameters.Add("@CreatedOn", office.CreatedOn);
                        parameters.Add("@LastEditedOn", office.LastEditedOn);
                        parameters.Add("@Enabled", office.Enabled);
                        parameters.Add("@Latitude", office.Latitude);
                        parameters.Add("@Longitude", office.Longitude);
                        parameters.Add("@RegisteredNo", office.RegisteredNo);
                        parameters.Add("@Description", office.Description);
                        parameters.Add("@AssetId", office.AssetId);
                        parameters.Add("@AddressType", office.AddressType);
                        parameters.Add("@StoreId", office.StoreId);
                        parameters.Add("@IsDefault", office.IsDefault);

                        result = connection.Execute("spCreateOrUpdateOffice", parameters, commandType: CommandType.StoredProcedure);
                    }
                }
                if (result != 0)
                {
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataSavedSuccessfully, "Offices") });
                }
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute() });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
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
                List<officeVm> officeRecords;
                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@DisplayLength", displayLength);
                        parameters.Add("@DisplayStart", displayStart);
                        parameters.Add("@SortCol", sortCol);
                        parameters.Add("@Search", search);
                        parameters.Add("@SortDir", sortAscending);
                        officeRecords = connection.Query<officeVm>("spGetOfficeData", parameters, commandType: CommandType.StoredProcedure).ToList();
                    }
                }
                officeRecords = officeRecords.ToList();
                office.Count = officeRecords.Count;
                office.OfficeList = officeRecords;
                return new JsonResult(new CustomResponse<OfficeModel>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = office });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute(), Data = ex });
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
                var officeExists = _CIRDbContext.offices.Any(x => x.Id == officeId);
                if (!officeExists)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
                }
                Offices office;
                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@Id", officeId);
                        office = connection.QueryFirst<Offices>("GetOfficesById", parameters, commandType: CommandType.StoredProcedure);
                    }
                }
                return new JsonResult(new CustomResponse<Offices>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = office });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }


        /// <summary>
        /// This method takes a delete holiday 
        /// </summary>
        /// <param name="officeId"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteOffice(long officeId)
        {
            try
            {
                if (officeId == null)
                {
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgAddingDataError, "Office") });
                }
                var result = 0;
                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@Id", officeId);
                        result = connection.Execute("spDeleteOffice", parameters, commandType: CommandType.StoredProcedure);
                    }
                }
                if (result != 0)
                {
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Deleted, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Deleted.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataDeletedSuccessfully, "Office") });
                }
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute() });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute(), Data = ex });
            }
        }
        #endregion
    }
}
