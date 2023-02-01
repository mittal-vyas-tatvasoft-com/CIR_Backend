using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
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
                    return new JsonResult(new CustomResponse<List<GlobalConfigurationReasons>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
                }
                return new JsonResult(new CustomResponse<List<GlobalMessagesModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = globalConfigurationMessagesList });

            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
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
                            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataSavedSuccessfully, "Global Configuration Messages") });
                        }
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute(), Data = SystemMessages.msgSomethingWentWrong });
                    }
                }
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }

        #endregion
    }
}
