using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                var globalConfigurationStyleList = await _CIRDBContext.GlobalConfigurationStyles.Select(x => new GlobalConfigurationStyle()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    TypeCode = x.TypeCode,
                    TypeName = x.TypeName,
                    ValueType = x.ValueType,
                    Value = x.Value,
                    SortOrder = x.SortOrder

                }).ToListAsync();
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
                    foreach (var item in globalConfigurationStyles)
                    {
                        GlobalConfigurationStyle newStyle = new GlobalConfigurationStyle()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Description = item.Description,
                            TypeCode = item.TypeCode,
                            TypeName = item.TypeName,
                            ValueType = item.ValueType,
                            Value = item.Value,
                            SortOrder = item.SortOrder
                        };
                        _CIRDBContext.GlobalConfigurationStyles.Update(newStyle);

                    }
                    await _CIRDBContext.SaveChangesAsync();
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = "GlobalConfiguration styles update successfully." });
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
