using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities;
using CIR.Core.Entities.Website;
using CIR.Core.Entities.Websites;
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
        public async Task<IActionResult> CreateorUpdatePortal(PortalModel portalModel, long clientId)
        {
            try
            {
                if (portalModel != null && clientId >= 0)
                {

                    if (portalModel.Id == 0)
                    {
                        return await CreatePortal(portalModel, clientId);
                    }
                    else
                    {
                        return await UpdatePortal(portalModel, clientId);
                    }
                }
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgAddingDataError, "Portal") });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }

        /// <summary>
        /// This method is used by CreateOrUpdate method of portal repository
        /// </summary>
        /// <param name="portalModel"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<IActionResult> CreatePortal(PortalModel portalModel, long clientId)
        {
            try
            {
                var existingClient = _CIRDbContext.Clients.FirstOrDefault(c => c.Id == clientId);
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
                _CIRDbContext.portals.Add(portal);
                await _CIRDbContext.SaveChangesAsync();

                var subsite = new SubSite()
                {
                    DisplayName = portalModel.DisplayName,
                    Directory = portalModel.Directory,
                    Domain = _CIRDbContext.SubSites.FirstOrDefault(x => x.Id == existingClient.SubsiteId).Domain,
                    Description = portalModel.Description,
                    Stopped = portalModel.Stopped,
                    EmailStopped = portalModel.EmailStopped,
                    SystemEmailFromAddress = portalModel.SystemEmailFromAddress,
                    BccEmailAddress = portalModel.BccEmailAddress,
                    Enabled = true,
                    ShowTax = false,
                    PortalId = portal.Id
                };
                _CIRDbContext.SubSites.Add(subsite);
                await _CIRDbContext.SaveChangesAsync();

                List<PortalServiceTypes> serviceTypes = new List<PortalServiceTypes>();
                var postalServiceType = new PortalServiceTypes()
                {
                    Enabled = portalModel.PostalServiceTypeEnabled,
                    Cost = portalModel.PostalServiceTypeCost,
                    Type = Convert.ToInt16(WebsiteEnums.ServiceTypes.Postal),
                    PortalId = portal.Id
                };
                serviceTypes.Add(postalServiceType);

                var dropoffServiceType = new PortalServiceTypes()
                {
                    Enabled = portalModel.DropOffServiceTypeEnabled,
                    Cost = portalModel.DropOffServiceTypeCost,
                    Type = Convert.ToInt16(WebsiteEnums.ServiceTypes.DropOff),
                    PortalId = portal.Id
                };
                serviceTypes.Add(dropoffServiceType);

                var collectionServiceType = new PortalServiceTypes()
                {
                    Enabled = portalModel.CollectionServiceTypeEnabled,
                    Cost = portalModel.CollectionServiceTypeCost,
                    Type = Convert.ToInt16(WebsiteEnums.ServiceTypes.Collection),
                    PortalId = portal.Id
                };
                serviceTypes.Add(collectionServiceType);

                _CIRDbContext.PortalServiceTypes.AddRange(serviceTypes);
                await _CIRDbContext.SaveChangesAsync();
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataSavedSuccessfully, "Portal") });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }

        /// <summary>
        /// This method is used by CreateOrUpdate method of portal repository
        /// </summary>
        /// <param name="portalModel"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdatePortal(PortalModel portalModel, long clientId)
        {
            try
            {
                var existingClient = _CIRDbContext.Clients.FirstOrDefault(c => c.Id == clientId);
                var existingPortal = _CIRDbContext.portals.FirstOrDefault(portal => portal.Id == portalModel.Id);
                if (existingPortal != null)
                {
                    _CIRDbContext.portals.Where(_ => _.Id == portalModel.Id).ToList().ForEach(portal =>
                    {
                        portal.CreateResponse = portalModel.CreateResponse;
                        portal.CountReturnIdentifier = portalModel.CountReturnIdentifier;
                        portal.CountryId = portalModel.CountryId;
                        portal.CurrencyId = portalModel.CurrencyId;
                        portal.CultureId = portalModel.CultureId;
                        portal.IntegrationLevel = portalModel.IntegrationLevel;
                        portal.ReturnItemsEnabled = false;
                        portal.ClientId = clientId;
                        portal.GlobalConfigurationFontId = _CIRDbContext.Fonts.FirstOrDefault(fontId => fontId.IsDefault).Id;
                        portal.Account = portalModel.Account;
                        portal.Entity = portalModel.Entity;
                    });
                    await _CIRDbContext.SaveChangesAsync();

                    var existingSubsite = _CIRDbContext.SubSites.FirstOrDefault(subsite => subsite.PortalId == existingPortal.Id);
                    var existingSubsiteId = existingSubsite.Id;

                    _CIRDbContext.SubSites.Where(_ => _.Id == existingSubsiteId).ToList().ForEach(subsite =>
                    {
                        subsite.DisplayName = portalModel.DisplayName;
                        subsite.Directory = portalModel.Directory;
                        subsite.Domain = _CIRDbContext.SubSites.FirstOrDefault(x => x.Id == existingClient.SubsiteId).Domain;
                        subsite.Description = portalModel.Description;
                        subsite.Stopped = portalModel.Stopped;
                        subsite.EmailStopped = portalModel.EmailStopped;
                        subsite.SystemEmailFromAddress = portalModel.SystemEmailFromAddress;
                        subsite.BccEmailAddress = portalModel.BccEmailAddress;
                        subsite.Enabled = true;
                        subsite.ShowTax = false;
                        subsite.PortalId = portalModel.Id;
                    });
                    await _CIRDbContext.SaveChangesAsync();

                    var serviceTypes = _CIRDbContext.PortalServiceTypes.Where(service => service.PortalId == portalModel.Id).ToList();

                    foreach (var serviceType in serviceTypes)
                    {
                        var serviceTypeId = serviceType.Id;

                        serviceType.Id = serviceTypeId;
                        serviceType.PortalId = existingPortal.Id;
                        if (serviceType.Type == 0)
                        {
                            serviceType.Enabled = portalModel.PostalServiceTypeEnabled;
                            serviceType.Cost = portalModel.PostalServiceTypeCost;
                        }
                        else if (serviceType.Type == 1)
                        {
                            serviceType.Enabled = portalModel.DropOffServiceTypeEnabled;
                            serviceType.Cost = portalModel.DropOffServiceTypeCost;
                        }
                        else if (serviceType.Type == 2)
                        {
                            serviceType.Enabled = portalModel.CollectionServiceTypeEnabled;
                            serviceType.Cost = portalModel.CollectionServiceTypeCost;
                        }
                        _CIRDbContext.PortalServiceTypes.Update(serviceType);
                        await _CIRDbContext.SaveChangesAsync();
                    }
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataUpdatedSuccessfully, "Portal") });
                }
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgIdNotFound, "Portal") });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
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
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NoContent, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.NoContent.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataDisabledSuccessfully, "Portal") });
                    }
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgIdNotFound, "Portal") });
                }
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }

        /// <summary>
        /// This method will return all the portals under given client
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetByClientId(int clientId)
        {
            if (clientId == null)
            {
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });
            }
            try
            {
                var clientPortals = (from portal in _CIRDbContext.portals
                                     join subsite in _CIRDbContext.SubSites
                                     on portal.Id equals subsite.PortalId
                                     select new ClientPortalsModel()
                                     {
                                         ClientId = portal.ClientId,
                                         PortalId = portal.Id,
                                         PortalName = subsite.DisplayName
                                     }).Where(x => x.ClientId == clientId).ToList();

                if (clientPortals.Count == 0)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
                }
                return new JsonResult(new CustomResponse<List<ClientPortalsModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = clientPortals });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }

        /// <summary>
        /// This method returns portal details of given portal Id
        /// </summary>
        /// <param name="portalId"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetById(int portalId)
        {
            if (portalId == null)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute() });
            }

            try
            {
                var serviceTypes = _CIRDbContext.PortalServiceTypes.Where(x => x.PortalId == portalId).ToList();
                if (serviceTypes.Count == 0)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
                }
                PortalServiceTypes postalServiceTypeDetail = GetServiceTypeDetails(serviceTypes, WebsiteEnums.ServiceTypes.Postal);
                PortalServiceTypes dropOffServiceTypeDetail = GetServiceTypeDetails(serviceTypes, WebsiteEnums.ServiceTypes.DropOff);
                PortalServiceTypes collectionServiceTypeDetail = GetServiceTypeDetails(serviceTypes, WebsiteEnums.ServiceTypes.Collection);

                var portalDetail = (from portal in _CIRDbContext.portals
                                    join subsite in _CIRDbContext.SubSites
                                    on portal.Id equals subsite.PortalId
                                    join servicetype in _CIRDbContext.PortalServiceTypes
                                    on portal.Id equals servicetype.PortalId
                                    select new PortalModel()
                                    {
                                        Id = portal.Id,
                                        ClientId = portal.ClientId,
                                        DisplayName = subsite.DisplayName,
                                        Directory = subsite.Directory,
                                        Domain = subsite.Domain,
                                        Description = subsite.Description,
                                        Stopped = subsite.Stopped,
                                        EmailStopped = subsite.EmailStopped,
                                        CreateResponse = portal.CreateResponse,
                                        CountReturnIdentifier = portal.CountReturnIdentifier,
                                        SystemEmailFromAddress = subsite.SystemEmailFromAddress,
                                        BccEmailAddress = subsite.BccEmailAddress,
                                        CurrencyId = portal.CurrencyId,
                                        CountryId = portal.CountryId,
                                        CultureId = portal.CultureId,
                                        IntegrationLevel = portal.IntegrationLevel,
                                        Entity = portal.Entity,
                                        Account = portal.Account,
                                        PostalServiceTypeEnabled = postalServiceTypeDetail.Enabled,
                                        PostalServiceTypeCost = postalServiceTypeDetail.Cost,
                                        DropOffServiceTypeEnabled = dropOffServiceTypeDetail.Enabled,
                                        DropOffServiceTypeCost = dropOffServiceTypeDetail.Cost,
                                        CollectionServiceTypeEnabled = collectionServiceTypeDetail.Enabled,
                                        CollectionServiceTypeCost = collectionServiceTypeDetail.Cost
                                    }).Where(x => x.Id == portalId).FirstOrDefault();

                if (portalDetail == null)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
                }
                return new JsonResult(new CustomResponse<PortalModel>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = portalDetail });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }

        /// <summary>
        /// Returns relevant service type details
        /// </summary>
        /// <param name="serviceTypes"></param>
        /// <param name="serviceTypeName"></param>
        /// <returns></returns>
        public PortalServiceTypes GetServiceTypeDetails(List<PortalServiceTypes> serviceTypes, WebsiteEnums.ServiceTypes serviceTypeName)
        {
            var serviceTypeValue = Convert.ToInt32(serviceTypeName);
            foreach (var service in serviceTypes)
            {
                if (service.Type == serviceTypeValue)
                {
                    var portalServiceType = new PortalServiceTypes()
                    {
                        Enabled = service.Enabled,
                        Cost = service.Cost,
                    };
                    return portalServiceType;
                }
            }
            var serviceType = new PortalServiceTypes()
            {
                Enabled = serviceTypes.FirstOrDefault().Enabled,
                Cost = serviceTypes.FirstOrDefault().Cost
            };
            return serviceType;
        }
        #endregion
    }
}
