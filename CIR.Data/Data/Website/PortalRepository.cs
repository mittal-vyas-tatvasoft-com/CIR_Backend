using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Common.Websites;
using CIR.Core.Entities;
using CIR.Core.Entities.Website;
using CIR.Core.Interfaces.Website;
using CIR.Core.ViewModel.Website;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Data.Data.Website
{
    public class PortalRepository : IPortalRepository
    {
        #region PROPERTIES

        private readonly CIRDbContext _CIRDbContext;

        #endregion

        #region CONSTRUCTORS

        public PortalRepository(CIRDbContext context)
        {
            _CIRDbContext = context ??
                throw new ArgumentNullException(nameof(context));
        }

        #endregion

        #region METHODS

        /// <summary>
        /// This method is used by the create method of portals controller
        /// </summary>
        /// <param name="portalModel"></param>
        /// <param name="clientId"></param>
        public async Task<IActionResult> CreatePortal(PortalModel portalModel, long clientId)
        {
            try
            {
                if (portalModel != null && clientId > 0)
                {

                    var portal = new Portals()
                    {
                        CreateResponse = portalModel.CreateResponse,
                        CountReturnIdentifier = portalModel.CountReturnIdentifier,
                        CountryId = portalModel.CountryId,
                        CurrencyId = portalModel.CurrencyId,
                        CultureId = portalModel.CultureId,
                        IntegrationLevel = portalModel.IntegrationLevel,
                        ReturnItemsEnabled = false,
                        ClientId = clientId,
                        GlobalConfigurationFontId = _CIRDbContext.Fonts.FirstOrDefault(fontId => fontId.IsDefault).Id,
                        Account = portalModel.Account,
                        Entity = portalModel.Entity
                    };
                    _CIRDbContext.Portals.Add(portal);
                    _CIRDbContext.SaveChanges();

                    var subsite = new SubSite()
                    {
                        DisplayName = portalModel.DisplayName,
                        Directory = portalModel.Directory,
                        Description = portalModel.Description,
                        Stopped = portalModel.Stopped,
                        EmailStopped = portalModel.EmailStopped,
                        SystemEmailFromAddress = portalModel.SystemEmailFromAddress,
                        BccemailAddress = portalModel.BccemailAddress,
                        Enabled = true,
                        ShowTax = false,
                        PortalId = portal.Id
                    };
                    _CIRDbContext.SubSites.Add(subsite);
                    _CIRDbContext.SaveChanges();

                    List<PortalServiceTypes> serviceTypes = new List<PortalServiceTypes>();
                    var postalServiceType = new PortalServiceTypes()
                    {
                        Enabled = portalModel.PostalServiceTypeEnabled,
                        Cost = portalModel.PostalServiceTypeCost,
                        Type = Convert.ToInt16(ServiceTypes.Postal),
                        PortalId = portal.Id
                    };
                    serviceTypes.Add(postalServiceType);

                    var dropoffServiceType = new PortalServiceTypes()
                    {
                        Enabled = portalModel.DropOffServiceTypeEnabled,
                        Cost = portalModel.DropOffServiceTypeCost,
                        Type = Convert.ToInt16(ServiceTypes.DropOff),
                        PortalId = portal.Id
                    };
                    serviceTypes.Add(dropoffServiceType);

                    var collectionServiceType = new PortalServiceTypes()
                    {
                        Enabled = portalModel.CollectionServiceTypeEnabled,
                        Cost = portalModel.CollectionServiceTypeCost,
                        Type = Convert.ToInt16(ServiceTypes.Collection),
                        PortalId = portal.Id
                    };
                    serviceTypes.Add(collectionServiceType);

                    _CIRDbContext.PortalServiceTypes.AddRange(serviceTypes);
                    _CIRDbContext.SaveChanges();

                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = "Portal Added Successfully" });


                }
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Error occurred while adding new portal" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        /// <summary>
        /// This method is used by delete method of portals controller
        /// </summary>
        /// <param name="portalId"></param>
        /// <returns>Returns No content if successful or returns not found or bad request</returns>
        public async Task<IActionResult> DisablePortal(long portalId)
        {
            try
            {
                if (portalId > 0)
                {
                    var portalSubsite = _CIRDbContext.SubSites.FirstOrDefault(subsite => subsite.PortalId == portalId);
                    if (portalSubsite != null)
                    {
                        portalSubsite.Enabled = false;
                        _CIRDbContext.SaveChanges();
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.NoContent, Result = true, Message = HttpStatusCodesMessages.NoContent, Data = "Portal Disabled Successfully." });
                    }
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = "Portal with given id not found." });
                }
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Error" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        #endregion
    }
}
