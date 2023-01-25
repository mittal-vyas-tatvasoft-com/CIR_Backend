using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Interfaces.GlobalConfiguration;
using CIR.Core.ViewModel.GlobalConfiguration;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CIR.Data.Data.GlobalConfiguration
{
    public class GlobalConfigurationCurrenciesRepository : IGlobalConfigurationCurrenciesRepository
    {
        #region PROPERTIES

        private readonly CIRDbContext _CIRDBContext;

        #endregion

        #region CONSTRUCTORS
        public GlobalConfigurationCurrenciesRepository(CIRDbContext context)
        {
            _CIRDBContext = context ??
                throw new ArgumentNullException(nameof(context));
        }

        #endregion

        #region METHODS

        /// <summary>
        /// This method used by getcurrency List countryid wise
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetGlobalConfigurationCurrenciesCountryWise(int countryId)
        {
            try
            {
                List<GlobalConfigurationCurrencyModel> globalConfigurationCurrenciesList;

                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("countryId", countryId);
                        globalConfigurationCurrenciesList = connection.Query<GlobalConfigurationCurrencyModel>("spGetGlobalConfigurationCurrenciesCountryWise", parameters, commandType: CommandType.StoredProcedure).ToList();
                    }
                }

                if (globalConfigurationCurrenciesList.Count == 0)
                {
                    return new JsonResult(new CustomResponse<List<GlobalConfigurationCurrencyModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
                }
                return new JsonResult(new CustomResponse<List<GlobalConfigurationCurrencyModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = globalConfigurationCurrenciesList });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }

        /// <summary>
        /// This method is used by create method and update method of globalcurrency controller
        /// </summary>
        /// <param name="globalCurrencyModel"></param>
        /// <returns>Success status if its valid else failure</returns>

        public async Task<IActionResult> CreateOrUpdateGlobalConfigurationCurrencies(List<GlobalCurrencyModel> globalCurrencyModel)
        {
            try
            {
                if (globalCurrencyModel.Any(x => x.CountryId == 0 || x.CurrencyId == 0))
                {
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgEnterValidData });
                }
                if (globalCurrencyModel != null)
                {
                    var result = 0;
                    foreach (var item in globalCurrencyModel)
                    {
                        using (DbConnection dbConnection = new DbConnection())
                        {
                            using (var connection = dbConnection.Connection)
                            {
                                DynamicParameters parameters = new DynamicParameters();
                                parameters.Add("@Id", item.Id);
                                parameters.Add("@CountryId", item.CountryId);
                                parameters.Add("@CurrencyId", item.CurrencyId);
                                parameters.Add("@Enabled", item.Enabled);

                                result = connection.Execute("spCreateOrUpdateGlobalConfigurationCurrencies", parameters, commandType: CommandType.StoredProcedure);
                            }
                        }
                    }
                    if (result != 0)
                    {
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataSavedSuccessfully, "Global Currency") });
                    }
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute() });
                }
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgAddingDataError, "Global Currency") });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }
        #endregion
    }
}
