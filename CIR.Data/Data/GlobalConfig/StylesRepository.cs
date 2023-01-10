using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Entities.Users;
using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.ViewModel.GlobalConfig;
using CIR.Core.ViewModel.Usersvm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Data.Data.GlobalConfig
{
    public class StylesRepository : ControllerBase, IStylesRepository
    {
        #region PROPERTIES   
        private readonly CIRDbContext _CIRDBContext;
        #endregion

        #region CONSTRUCTOR
        public StylesRepository(CIRDbContext context)
        {
            _CIRDBContext = context ??
               throw new ArgumentNullException(nameof(context));
        }
        #endregion

        #region METHODS
        /// <summary>
        /// This method retuns Style list using LINQ
        /// </summary>
        /// <returns> list of style </returns>


        public async Task<GlobalConfigurationStyleModel> GetAllStyles()
        {
            GlobalConfigurationStyleModel styles = new();
            try
            {
                styles.StylesList = _CIRDBContext.GlobalConfigurationStyles.ToList();
                return styles;
            }
            catch (Exception ex)
            {
                return styles;
            }

        }

        /// <summary>
        /// This method is used by save method of Styles controller
        /// </summary>
        /// <param name="model"> save Style data </param>
        /// <returns> Ok status if its valid else unprocessable </returns>
       
        public async Task<IActionResult> SaveStyle(List<GlobalConfigurationStyle> model)
        {
            try
            {
                if (model != null)
                {
                    foreach (var item in model)
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
                    _CIRDBContext.SaveChanges();
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success});
                }
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.NoContent, Result = false, Message = HttpStatusCodesMessages.NoContent });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }

        }

        #endregion
    }
}
