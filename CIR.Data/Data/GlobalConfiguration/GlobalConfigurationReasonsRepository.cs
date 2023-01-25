using CIR.Common.CustomResponse;
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
    public class GlobalConfigurationReasonsRepository : IGlobalConfigurationReasonsRepository
    {
        #region PROPERTIES

        private readonly CIRDbContext _CIRDbContext;

        #endregion

        #region CONSTRUCTORS

        public GlobalConfigurationReasonsRepository(CIRDbContext context)
        {
            _CIRDbContext = context ??
                throw new ArgumentNullException(nameof(context));
        }

        #endregion

        #region METHODS

        /// <summary>
        /// This method takes a get globalconfiguration reasons list
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetGlobalConfigurationReasons()
        {
            try
            {
                List<GlobalConfigurationReasons> globalConfigurationReasonsList;
                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        globalConfigurationReasonsList = connection.Query<GlobalConfigurationReasons>("spGetGlobalConfigurationReasons", null, commandType: CommandType.StoredProcedure).ToList();
                    }
                }

                if (globalConfigurationReasonsList.Count == 0)
                {
                    return new JsonResult(new CustomResponse<List<GlobalConfigurationReasons>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute(), Data = null });
                }

                return new JsonResult(new CustomResponse<List<GlobalConfigurationReasons>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = globalConfigurationReasonsList });

            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }


        /// <summary>
        /// This method takes a create or update globalconfiguration reasons
        /// </summary>
        /// <param name="globalConfigurationReasons"></param>
        /// <returns></returns>
        public async Task<IActionResult> CreateOrUpdateGlobalConfigurationReasons(List<GlobalConfigurationReasons> globalConfigurationReasons)
        {
            try
            {
                if (globalConfigurationReasons != null)
                {
                    var result = 0;
                    foreach (var option in globalConfigurationReasons)
                    {
                        using (DbConnection dbConnection = new DbConnection())
                        {
                            using (var connection = dbConnection.Connection)
                            {
                                DynamicParameters parameters = new DynamicParameters();
                                parameters.Add("@Id", option.Id);
                                parameters.Add("@Type", option.Type);
                                parameters.Add("@Enabled", option.Enabled);
                                parameters.Add("@Content", option.Content);

                                result = connection.Execute("spCreateOrUpdateGlobalConfigurationReasons", parameters, commandType: CommandType.StoredProcedure);
                            }
                        }
                    }
                    if (result != 0)
                    {
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = "GlobalConfiguration Reason saved successfully" });
                    }
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute(), Data = "error" });

                }
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = "error" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }

        #endregion
    }
}
