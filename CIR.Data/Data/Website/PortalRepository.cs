using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities.Website;
using CIR.Core.Interfaces.Website;
using CIR.Core.ViewModel.Website;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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
        /// This method is used by Create method of portal controller
        /// </summary>
        /// <param name="portalModel"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<IActionResult> CreatePortal(PortalModel portalModel, long clientId)
        {
            try
            {
                var result = 0;
                var existingClient = _CIRDbContext.Clients.FirstOrDefault(c => c.Id == clientId);

                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@CreateResponse", portalModel.CreateResponse);
                        parameters.Add("@CountReturnIdentifier", portalModel.CountReturnIdentifier);
                        parameters.Add("@CountryId", portalModel.CountryId);
                        parameters.Add("CurrencyId", portalModel.CurrencyId);
                        parameters.Add("@CultureId", portalModel.CultureId);
                        parameters.Add("@IntegrationLevel", portalModel.IntegrationLevel);
                        parameters.Add("@ReturnItemsEnabled", false);
                        parameters.Add("@ClientId", clientId);
                        parameters.Add("@GlobalConfigurationFontId", _CIRDbContext.Fonts.FirstOrDefault(fontId => fontId.IsDefault).Id);
                        parameters.Add("@Account", portalModel.Account);
                        parameters.Add("@Entity", portalModel.Entity);

                        result = connection.Execute("spCreatePortal", parameters, commandType: CommandType.StoredProcedure);
                    }
                    if (result == 0)
                    {
                        return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute() });
                    }
                }
                long portalId = _CIRDbContext.portals.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@DisplayName", portalModel.DisplayName);
                        parameters.Add("@Directory", portalModel.Directory);
                        parameters.Add("@Domain", _CIRDbContext.SubSites.FirstOrDefault(x => x.Id == existingClient.SubsiteId).Domain);
                        parameters.Add("@Description", portalModel.Description);
                        parameters.Add("@Stopped", portalModel.Stopped);
                        parameters.Add("@EmailStopped", portalModel.EmailStopped);
                        parameters.Add("@SystemEmailFromAddress", portalModel.SystemEmailFromAddress);
                        parameters.Add("@BccEmailAddress", portalModel.BccEmailAddress);
                        parameters.Add("@Enabled", true);
                        parameters.Add("@ShowTax", false);
                        parameters.Add("@PortalId", portalId);

                        result = connection.Execute("spCreateSubsite", parameters, commandType: CommandType.StoredProcedure);
                    }
                    if (result == 0)
                    {
                        return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute() });
                    }
                }

                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@Enabled", portalModel.PostalServiceTypeEnabled);
                        parameters.Add("@Cost", portalModel.PostalServiceTypeCost);
                        parameters.Add("@PortalId", portalId);
                        parameters.Add("@Type", Convert.ToInt16(WebsiteEnums.ServiceTypes.Postal));

                        result = connection.Execute("spAddServiceTypes", parameters, commandType: CommandType.StoredProcedure);
                    }
                    if (result == 0)
                    {
                        return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute() });
                    }
                }

                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@Enabled", portalModel.DropOffServiceTypeEnabled);
                        parameters.Add("@Cost", portalModel.DropOffServiceTypeCost);
                        parameters.Add("@PortalId", portalId);
                        parameters.Add("@Type", Convert.ToInt16(WebsiteEnums.ServiceTypes.DropOff));

                        result = connection.Execute("spAddServiceTypes", parameters, commandType: CommandType.StoredProcedure);
                    }
                    if (result == 0)
                    {
                        return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute() });
                    }
                }

                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@Enabled", portalModel.CollectionServiceTypeEnabled);
                        parameters.Add("@Cost", portalModel.CollectionServiceTypeCost);
                        parameters.Add("@PortalId", portalId);
                        parameters.Add("@Type", Convert.ToInt16(WebsiteEnums.ServiceTypes.Collection));

                        result = connection.Execute("spAddServiceTypes", parameters, commandType: CommandType.StoredProcedure);
                    }
                    if (result == 0)
                    {
                        return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute() });
                    }
                }
                var portalIdObj = new Dictionary<string, object>
                {
                    { "Id", portalId }
                };
                return new JsonResult(new CustomResponse<Dictionary<string, object>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = portalIdObj });
            }

            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }

        /// <summary>
        /// This method is used by Update method of portal controller
        /// </summary>
        /// <param name="portalModel"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdatePortal(PortalModel portalModel)
        {
            try
            {
                var result = 0;
                var existingPortal = _CIRDbContext.portals.FirstOrDefault(portal => portal.Id == portalModel.Id);
                if (existingPortal != null)
                {
                    using (DbConnection dbConnection = new DbConnection())
                    {
                        using (var connection = dbConnection.Connection)
                        {
                            DynamicParameters parameters = new DynamicParameters();
                            parameters.Add("@Id", existingPortal.Id);
                            parameters.Add("@CreateResponse", portalModel.CreateResponse);
                            parameters.Add("@CountReturnIdentifier", portalModel.CountReturnIdentifier);
                            parameters.Add("@CountryId", portalModel.CountryId);
                            parameters.Add("CurrencyId", portalModel.CurrencyId);
                            parameters.Add("@CultureId", portalModel.CultureId);
                            parameters.Add("@IntegrationLevel", portalModel.IntegrationLevel);
                            parameters.Add("@ReturnItemsEnabled", false);
                            parameters.Add("@GlobalConfigurationFontId", _CIRDbContext.Fonts.FirstOrDefault(fontId => fontId.IsDefault).Id);
                            parameters.Add("@Account", portalModel.Account);
                            parameters.Add("@Entity", portalModel.Entity);

                            result = connection.Execute("spUpdatePortal", parameters, commandType: CommandType.StoredProcedure);
                        }
                        if (result == 0)
                        {
                            return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute() });
                        }
                    }

                    var existingSubsite = _CIRDbContext.SubSites.FirstOrDefault(subsite => subsite.PortalId == existingPortal.Id);
                    var existingSubsiteId = existingSubsite.Id;

                    using (DbConnection dbConnection = new DbConnection())
                    {
                        using (var connection = dbConnection.Connection)
                        {
                            DynamicParameters parameters = new DynamicParameters();
                            parameters.Add("@SubsiteId", existingSubsiteId);
                            parameters.Add("@DisplayName", portalModel.DisplayName);
                            parameters.Add("@Directory", portalModel.Directory);
                            parameters.Add("@Description", portalModel.Description);
                            parameters.Add("@Stopped", portalModel.Stopped);
                            parameters.Add("@EmailStopped", portalModel.EmailStopped);
                            parameters.Add("@SystemEmailFromAddress", portalModel.SystemEmailFromAddress);
                            parameters.Add("@BccEmailAddress", portalModel.BccEmailAddress);
                            parameters.Add("@Enabled", true);
                            parameters.Add("@ShowTax", false);

                            result = connection.Execute("spUpdateSubsite", parameters, commandType: CommandType.StoredProcedure);
                        }
                        if (result == 0)
                        {
                            return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute() });
                        }
                    }

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
                        using (DbConnection dbConnection = new DbConnection())
                        {
                            using (var connection = dbConnection.Connection)
                            {
                                DynamicParameters parameters = new DynamicParameters();
                                parameters.Add("@Id", serviceType.Id);
                                parameters.Add("@Enabled", serviceType.Enabled);
                                parameters.Add("@Cost", serviceType.Cost);
                                parameters.Add("@Type", serviceType.Type);

                                result = connection.Execute("spUpdateServiceTypes", parameters, commandType: CommandType.StoredProcedure);
                            }

                        }
                    }
                    if (result == 0)
                    {
                        return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute() });
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
                    var result = 0;
                    var portalSubsite = _CIRDbContext.SubSites.FirstOrDefault(subsite => subsite.PortalId == portalId);
                    if (portalSubsite != null)
                    {
                        using (DbConnection dbConnection = new DbConnection())
                        {
                            using (var connection = dbConnection.Connection)
                            {
                                DynamicParameters parameters = new DynamicParameters();
                                parameters.Add("@Id", portalSubsite.Id);

                                result = connection.Execute("spDisablePortal", parameters, commandType: CommandType.StoredProcedure);
                            }
                            if (result == 0)
                            {
                                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute() });
                            }
                        }
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
                var clientPortals = new List<ClientPortalsModel>();

                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@clientId", clientId);
                        clientPortals = connection.Query<ClientPortalsModel>("spGetByClientId", parameters, commandType: CommandType.StoredProcedure).ToList();
                    }
                }

                if (clientPortals.Count == 0)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NoContent, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NoContent.GetDescriptionAttribute() });
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
            if (portalId == null && (!_CIRDbContext.portals.Any(x => x.Id == portalId)))
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

                var portalDetail = new PortalModel();
                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@portalId", portalId);
                        portalDetail = connection.QueryFirst<PortalModel>("spGetPortalById", parameters, commandType: CommandType.StoredProcedure);
                    }
                }
                portalDetail.PostalServiceTypeEnabled = postalServiceTypeDetail.Enabled;
                portalDetail.PostalServiceTypeCost = postalServiceTypeDetail.Cost;
                portalDetail.DropOffServiceTypeEnabled = dropOffServiceTypeDetail.Enabled;
                portalDetail.DropOffServiceTypeCost = dropOffServiceTypeDetail.Cost;
                portalDetail.CollectionServiceTypeEnabled = collectionServiceTypeDetail.Enabled;
                portalDetail.CollectionServiceTypeCost = collectionServiceTypeDetail.Cost;
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
