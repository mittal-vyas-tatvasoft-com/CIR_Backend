﻿using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfig;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CIR.Data.Data.GlobalConfig
{
    public class DropdownOptionRepository : IDropdownOptionRepository
    {
        #region PROPERTIES

        private readonly CIRDbContext _CIRDbContext;

        #endregion

        #region CONSTRUCTORS

        public DropdownOptionRepository(CIRDbContext context)
        {
            _CIRDbContext = context ??
                throw new ArgumentNullException(nameof(context));
        }

        #endregion

        #region METHODS

        /// <summary>
        /// this method is used by Get method of Dropdown options controller
        /// </summary>
        /// <returns>list of global congig reasons</returns>
        public async Task<IActionResult> GetAllDropdownOptions()
        {
            using (DbConnection dbConnection = new DbConnection())
            {

                using (var connection = dbConnection.Connection)
                {
                    var globalConfigReasons = await connection.QueryMultipleAsync("spGetAllDropdownOptions", null, commandType: CommandType.StoredProcedure);
                    var dropdownOptions = globalConfigReasons.Read<GlobalConfigurationReasons>().ToList();

                    if (dropdownOptions.Count > 0)
                    {
                        return new JsonResult(new CustomResponse<List<GlobalConfigurationReasons>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = dropdownOptions });
                    }
                    else
                    {
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.NoContent, Result = true, Message = HttpStatusCodesMessages.NoContent, Data = "No Data is present" });
                    }
                }
            }

        }


        /// <summary>
        /// This method is used by create or update method of dropdown options controller
        /// </summary>
        /// <param name="dropdownOptions"></param>
        /// <returns>return ok if successful else returns bad request</returns>
        public async Task<IActionResult> CreateOrUpdateDrownOption(List<GlobalConfigurationReasons> dropdownOptions)
        {

            if (dropdownOptions != null)
            {
                foreach (var option in dropdownOptions)
                {
                    if (option.Type <= 0 || option.Type > 3)
                    {
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Error occurred while adding new Global Currency" });

                    }
                    using (DbConnection dbConnection = new DbConnection())
                    {

                        using (var connection = dbConnection.Connection)
                        {
                            DynamicParameters parameters = new DynamicParameters();
                            parameters.Add("@Id", option.Id);
                            parameters.Add("@Type", option.Type);
                            parameters.Add("@Enabled", option.Enabled);
                            parameters.Add("@Content", option.Content);

                            connection.QueryMultiple("spAddorUpdateDropDownOptions", parameters, commandType: CommandType.StoredProcedure);
                        }
                    }
                }
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated, Data = "DropDown Option created or updated Successfully" });
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Error occurred while adding new Global Currency" });

        }

        #endregion
    }
}