using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using CIR.Core.ViewModel.GlobalConfiguration;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CIR.Data.Data.GlobalConfiguration
{
    public class GlobalConfigurationMessagesRepository : ControllerBase, IGlobalConfigurationMessagesRepository
    {
        #region PROPERTIES   
        private readonly CIRDbContext _CIRDBContext;
        #endregion

        #region CONSTRUCTOR
        public GlobalConfigurationMessagesRepository(CIRDbContext context)
        {
            _CIRDBContext = context ??
               throw new ArgumentNullException(nameof(context));
        }
        #endregion

        #region METHODS

        /// <summary>
        /// This method used by get globalconfiguration messages list
        /// </summary>
        /// <param name="cultureId"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetGlobalConfigurationMessagesList(int cultureId)
        {
            try
            {
                List<GlobalMessagesModel> globalConfigurationMessagesList;
                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@CultureId", cultureId);
                        globalConfigurationMessagesList = connection.Query<GlobalMessagesModel>("spGetGlobalConfigurationMessagesList", parameters, commandType: CommandType.StoredProcedure).ToList();
                    }
                }
                if (globalConfigurationMessagesList.Count == 0)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound });
                }
                return new JsonResult(new CustomResponse<List<GlobalMessagesModel>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = globalConfigurationMessagesList });

            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        /// <summary>
        /// This method is used by create or update globalConfiguration Messages
        /// </summary>
        /// <param name="globalConfigurationMessages"></param>
        /// <returns>Success status if its valid else failure</returns>
        public async Task<IActionResult> CreateOrUpdateGlobalConfigurationMessages(List<GlobalConfigurationMessages> globalConfigurationMessages)
        {
            try
            {
                if (globalConfigurationMessages != null)
                {
                    var result = 0;
                    using (DbConnection dbConnection = new DbConnection())
                    {
                        using (var connection = dbConnection.Connection)
                        {
                            foreach (var item in globalConfigurationMessages)
                            {
                                DynamicParameters parameters = new DynamicParameters();
                                parameters.Add("@Id", item.Id);
                                parameters.Add("@Type", item.Type);
                                parameters.Add("@Content", item.Content);
                                parameters.Add("@CultureId", item.CultureId);
                                result = connection.Execute("spCreateOrUpdateGlobalConfigurationMessages", parameters, commandType: CommandType.StoredProcedure);
                            }
                        }
                        if (result != 0)
                        {
                            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated, Data = "GlobalConfiguration messages saved succesfully." });
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
