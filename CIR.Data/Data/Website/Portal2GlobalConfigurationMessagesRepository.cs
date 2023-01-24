using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.Website;
using CIR.Core.Interfaces.Website;
using CIR.Core.ViewModel.Website;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CIR.Data.Data.Website
{
    public class Portal2GlobalConfigurationMessagesRepository : ControllerBase, IPortal2GlobalConfigurationMessagesRepository
    {
        #region PROPERTIES
        private readonly CIRDbContext _CIRDbContext;
        #endregion

        #region CONSTRUCTOR
        public Portal2GlobalConfigurationMessagesRepository(CIRDbContext context)
        {
            _CIRDbContext = context ??
                throw new ArgumentNullException(nameof(context));
        }
        #endregion

        #region METHODS
        /// <summary>
        /// This method used by PortalToGlobalConfigurationMessagesList
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetPortalToGlobalConfigurationMessagesList(long portalId)
        {
            try
            {
                var portal2GlobalConfigurationMessagesList = await (from portal2GlobalConfigurationMessages in _CIRDbContext.Portal2GlobalConfigurationMessages
                                                                    select new Portal2GlobalConfigurationMessagesModel()
                                                                    {
                                                                        Id = portal2GlobalConfigurationMessages.Id,
                                                                        PortalId = portal2GlobalConfigurationMessages.PortalId,
                                                                        GlobalConfigurationMessageId = portal2GlobalConfigurationMessages.GlobalConfigurationMessageId,
                                                                        ValueOverride = portal2GlobalConfigurationMessages.ValueOverride
                                                                    }).Where(x => x.PortalId == portalId).ToListAsync();

                if (portal2GlobalConfigurationMessagesList != null)
                    return new JsonResult(new CustomResponse<List<Portal2GlobalConfigurationMessagesModel>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = portal2GlobalConfigurationMessagesList });
                else
                    return new JsonResult(new CustomResponse<List<Portal2GlobalConfigurationMessagesModel>>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        /// <summary>
        /// This method is used by create method and update method of PortalToGlobalConfigurationMessages controller
        /// </summary>
        /// <param name="portalToGlobalConfigurationMessages"></param>
        /// <returns>Success status if its valid else failure</returns>
        public async Task<IActionResult> CreateOrUpdatePortalToGlobalConfigurationMessages(List<Portal2GlobalConfigurationMessage> portal2GlobalConfigurationMessage)
        {
            try
            {
                if (portal2GlobalConfigurationMessage.Any(x => x.PortalId == 0))
                {
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest });
                }
                if (portal2GlobalConfigurationMessage != null)
                {
                    foreach (var item in portal2GlobalConfigurationMessage)
                    {
                        if (item.Id != 0)
                        {
                            Portal2GlobalConfigurationMessage messages = new Portal2GlobalConfigurationMessage()
                            {
                                Id = item.Id,
                                PortalId = item.PortalId,
                                GlobalConfigurationMessageId = item.GlobalConfigurationMessageId,
                                ValueOverride = item.ValueOverride
                            };
                            _CIRDbContext.Portal2GlobalConfigurationMessages.Update(messages);
                        }
                        else
                        {
                            Portal2GlobalConfigurationMessage messages = new Portal2GlobalConfigurationMessage()
                            {
                                Id = item.Id,
                                PortalId = item.PortalId,
                                GlobalConfigurationMessageId = item.GlobalConfigurationMessageId,
                                ValueOverride = item.ValueOverride
                            };
                            _CIRDbContext.Portal2GlobalConfigurationMessages.Add(messages);
                        }
                    }
                    await _CIRDbContext.SaveChangesAsync();
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = "Portal To Global Configration messages saved succesfully." });
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
