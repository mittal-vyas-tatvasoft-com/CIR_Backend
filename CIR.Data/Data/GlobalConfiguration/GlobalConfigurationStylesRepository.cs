using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
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

                if (globalConfigurationStyleList.Count == 0)
                {
					return new JsonResult(new CustomResponse<List<GlobalConfigurationStyle>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}
				return new JsonResult(new CustomResponse<List<GlobalConfigurationStyle>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = globalConfigurationStyleList });
			}
            catch (Exception ex)
            {
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
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
                        GlobalConfigurationStyle newGlobalConfigStyle = new GlobalConfigurationStyle()
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
                        _CIRDBContext.GlobalConfigurationStyles.Update(newGlobalConfigStyle);

                    }
                    await _CIRDBContext.SaveChangesAsync();
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataUpdatedSuccessfully, "GlobalConfiguration Styles") });
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
