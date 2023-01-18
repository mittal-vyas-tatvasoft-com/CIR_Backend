using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.Website;
using CIR.Core.Interfaces.Website;
using CIR.Core.ViewModel.Website;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CIR.Data.Data.Website
{
    public class PortalToGlobalConfigurationEmailsRepository : ControllerBase, IPortalToGlobalConfigurationEmailsRepository
    {
        #region PROPERTIES

        private readonly CIRDbContext _CIRDBContext;

        #endregion
        #region CONSTRUCTOR
        public PortalToGlobalConfigurationEmailsRepository(CIRDbContext context)
        {
            _CIRDBContext = context ??
                throw new ArgumentNullException(nameof(context));
        }
        #endregion
        #region METHODS
        /// <summary>
        /// This method used by PortalToGlobalConfigurationEmails list
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetPortalToGlobalConfigurationEmailsList(int id)
        {
            try
            {
                var result = (from portal2GlobalConfigurationEmails in _CIRDBContext.Portal2GlobalConfigurationEmails
                              select new PortalToGlobalConfigurationEmailsGetModel()
                              {
                                  Id = portal2GlobalConfigurationEmails.Id,
                                  PortalId = portal2GlobalConfigurationEmails.PortalId,
                                  GlobalConfigurationEmailId = portal2GlobalConfigurationEmails.GlobalConfigurationEmailId,
                                  ContentOverride = portal2GlobalConfigurationEmails.ContentOverride,
                                  SubjectOverride = portal2GlobalConfigurationEmails.SubjectOverride,

                              }).Where(x => x.Id == id).ToList();

                var emailId = await _CIRDBContext.Portal2GlobalConfigurationEmails.Where(x => x.Id == id).FirstOrDefaultAsync();
                var serializedParent = JsonConvert.SerializeObject(emailId);
                PortalToGlobalConfigurationEmailsGetModel email = JsonConvert.DeserializeObject<PortalToGlobalConfigurationEmailsGetModel>(serializedParent);

                if (email.ContentOverride.Contains("[Reference]") || email.SubjectOverride.Contains("[Reference]"))
                {
                    email.Reference = true;
                }
                if (email.ContentOverride.Contains("[BookingId]") || email.SubjectOverride.Contains("[BookingId]"))
                {
                    email.BookingId = true;
                }
                if (email.ContentOverride.Contains("[OrderNumber]") || email.SubjectOverride.Contains("[OrderNumber]"))
                {
                    email.OrderNumber = true;
                }
                if (email.ContentOverride.Contains("[CollectionDate]") || email.SubjectOverride.Contains("[CollectionDate]"))
                {
                    email.CollectionDate = true;
                }
                if (email.ContentOverride.Contains("[CustomerEmail]") || email.SubjectOverride.Contains("[CustomerEmail]"))
                {
                    email.CustomerEmail = true;
                }
                if (email.ContentOverride.Contains("[CustomerName]") || email.SubjectOverride.Contains("[CustomerName]"))
                {
                    email.CustomerName = true;
                }
                if (email.ContentOverride.Contains("[CollectionAddress]") || email.SubjectOverride.Contains("[CollectionAddress]"))
                {
                    email.CollectionAddress = true;
                }
                if (email.ContentOverride.Contains("[TrackingURL]") || email.SubjectOverride.Contains("[TrackingURL]"))
                {
                    email.TrackingURL = true;
                }
                if (email.ContentOverride.Contains("[LabelURL]") || email.SubjectOverride.Contains("[LabelURL]"))
                {
                    email.LabelURL = true;
                }
                if (email.ContentOverride.Contains("[BookingURL]") || email.SubjectOverride.Contains("[BookingURL]"))
                {
                    email.BookingURL = true;
                }
                if (result != null)
                    return new JsonResult(new CustomResponse<List<PortalToGlobalConfigurationEmailsGetModel>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = result });
                else
                    return new JsonResult(new CustomResponse<List<PortalToGlobalConfigurationEmailsGetModel>>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound });

            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }
        /// <summary>
		/// This method is used by create method and update method of portalToGlobalConfigurationEmails controller
		/// </summary>
		/// <param name="portalToGlobalConfigurationEmails"></param>
		/// <returns>Success status if its valid else failure</returns>
        public async Task<IActionResult> CreateOrUpdatePortalToGlobalConfigurationEmails(List<PortalToGlobalConfigurationEmails> portalToGlobalConfigurationEmails)
        {
            try
            {
                if (portalToGlobalConfigurationEmails.Any(x => x.Id == 0))
                {
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest });
                }
                if (portalToGlobalConfigurationEmails != null)
                {
                    foreach (var item in portalToGlobalConfigurationEmails)
                    {
                        if (item.Id != 0)
                        {
                            var emailsCheck = _CIRDBContext.Portal2GlobalConfigurationEmails.FirstOrDefault(x => x.Id == item.Id);
                            if (emailsCheck != null)
                            {
                                emailsCheck.PortalId = item.PortalId;
                                emailsCheck.GlobalConfigurationEmailId = item.GlobalConfigurationEmailId;
                                emailsCheck.ContentOverride = item.ContentOverride;
                                emailsCheck.SubjectOverride = item.SubjectOverride;
                            }
                            else
                            {
                                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest });
                            }
                        }
                        else
                        {
                            PortalToGlobalConfigurationEmails email = new PortalToGlobalConfigurationEmails()
                            {
                                Id = item.Id,
                                PortalId = item.PortalId,
                                GlobalConfigurationEmailId = item.GlobalConfigurationEmailId,
                                ContentOverride = item.ContentOverride,
                                SubjectOverride = item.SubjectOverride
                            };
                            _CIRDBContext.Portal2GlobalConfigurationEmails.Add(email);
                        }
                    }
                    await _CIRDBContext.SaveChangesAsync();
                    return new JsonResult(new CustomResponse<List<PortalToGlobalConfigurationEmails>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success });
                }
                return new JsonResult(new CustomResponse<PortalToGlobalConfigurationEmails>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }
        #endregion
    }
}
