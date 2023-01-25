using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CIR.Data.Data.GlobalConfiguration
{
    public class GlobalConfigurationStylesRepository : ControllerBase, IGlobalConfigurationStylesRepository
    {
        #region PROPERTIES   
        private readonly CIRDbContext _CIRDBContext;
        #endregion

        #region CONSTRUCTOR
        public GlobalConfigurationStylesRepository(CIRDbContext context)
        {
            _CIRDBContext = context ??
               throw new ArgumentNullException(nameof(context));
        }
        #endregion

        #region METHODS

        /// <summary>
        /// This method used by get globalconfiguration styles list
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetGlobalConfigurationStyles()
        {
            try
            {
                List<GlobalConfigurationStyle> globalConfigurationStyleList;
                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        globalConfigurationStyleList = connection.Query<GlobalConfigurationStyle>("spGetGlobalConfigurationStyles", null, commandType: CommandType.StoredProcedure).ToList();
                    }
                }

                if (globalConfigurationStyleList.Count == 0)
                {
                    return new JsonResult(new CustomResponse<List<GlobalConfigurationStyle>>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = null });
                }

                return new JsonResult(new CustomResponse<List<GlobalConfigurationStyle>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = globalConfigurationStyleList });

            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }


        /// <summary>
        /// This method used by update globalconfiguration styles
        /// </summary>
        /// <param name="globalConfigurationStyles"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdateGlobalConfigurationStyles(List<GlobalConfigurationStyle> globalConfigurationStyles)
        {
            try
            {
                if (globalConfigurationStyles != null)
                {
                    var result = 0;
                    using (DbConnection dbConnection = new DbConnection())
                    {
                        using (var connection = dbConnection.Connection)
                        {
                            foreach (var item in globalConfigurationStyles)
                            {
                                DynamicParameters parameters = new DynamicParameters();
                                parameters.Add("@Id", item.Id);
                                parameters.Add("@Name", item.Name);
                                parameters.Add("@Description", item.Description);
                                parameters.Add("@TypeCode", item.TypeCode);
                                parameters.Add("@TypeName", item.TypeName);
                                parameters.Add("@ValueType", item.ValueType);
                                parameters.Add("@Value", item.Value);
                                parameters.Add("@SortOrder", item.SortOrder);
                                result = connection.Execute("spUpdateGlobalConfigurationStyles", parameters, commandType: CommandType.StoredProcedure);
                            }
                        }
                        if (result != 0)
                        {
                            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated, Data = "GlobalConfiguration styles update successfully." });
                        }
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = false, Message = HttpStatusCodesMessages.UnprocessableEntity, Data = "Something went wrong!" });
                    }
                }
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        #endregion
    }
}
