using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Interfaces.Website;
using CIR.Core.ViewModel.Website;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CIR.Data.Data.Website
{
    public class ClientRepository : IClientRepository
    {
        #region PROPERTIES

        private readonly CIRDbContext _CIRDbContext;

        #endregion

        #region CONSTRUCTORS

        public ClientRepository(CIRDbContext context)
        {
            _CIRDbContext = context ??
                throw new ArgumentNullException(nameof(context));
        }

        #endregion

        #region METHODS

        /// <summary>
        /// this method will be used by GetAll method of clients controller
        /// </summary>
        /// <returns>returns list of all the clients</returns>
        public async Task<IActionResult> GetAllClients()
        {
            try
            {
                List<ClientModel> clients;
                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        clients = connection.Query<ClientModel>("spGetAllClientDetails", null, commandType: CommandType.StoredProcedure).ToList();
                    }
                }
                if (clients.Count == 0)
                {
                    return new JsonResult(new CustomResponse<List<ClientModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
                }
                else
                {
                    return new JsonResult(new CustomResponse<List<ClientModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = clients });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }

        /// <summary>
        /// This method will be used by create method of clients controller
        /// </summary>
        /// <param name="clientModel"></param>
        /// <returns>return Ok if successful else returns bad request</returns>
        public async Task<IActionResult> CreateOrUpdateClient(ClientModel clientModel)
        {
            try
            {
                if (clientModel != null)
                {
                    if (clientModel.Id < 0)
                    {
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgAddingDataError, "Client") });
                    }
                    else if (clientModel.Id == 0)
                    {
                        return await CreateClient(clientModel);
                    }
                    else
                    {
                        return await UpdateClient(clientModel);
                    }
                }
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgAddingDataError, "Client") });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }

        /// <summary>
        /// This method is used by CreateOrUpdate method of Client Repository
        /// </summary>
        /// <param name="clientModel"></param>
        /// <returns></returns>
        public async Task<IActionResult> CreateClient(ClientModel clientModel)
        {
            try
            {
                var result = 0;
                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@DisplayName", clientModel.Name);
                        parameters.Add("@Domain", clientModel.Domain);
                        parameters.Add("@Description", clientModel.Description);
                        parameters.Add("@Stopped", clientModel.Stopped);
                        parameters.Add("@EmailStopped", clientModel.EmailStopped);

                        result = connection.Execute("spCreateSubsiteForClient", parameters, commandType: CommandType.StoredProcedure);
                    }
                }
                if (result == 0)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute() });
                }

                var subsiteId = _CIRDbContext.SubSites.OrderByDescending(x => x.Id).FirstOrDefault().Id;

                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@Name", clientModel.Name);
                        parameters.Add("@SubsiteId", subsiteId);
                        parameters.Add("@Code", clientModel.Code);

                        result = connection.Execute("spCreateClient", parameters, commandType: CommandType.StoredProcedure);
                    }
                }
                if (result == 0)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute() });
                }
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataSavedSuccessfully, "Client") });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }

        /// <summary>
        /// This method is used by CreateOrUpdate method of Client Repository
        /// </summary>
        /// <param name="clientModel"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdateClient(ClientModel clientModel)
        {
            try
            {
                var result = 0;
                var existingClient = _CIRDbContext.Clients.FirstOrDefault(client => client.Id == clientModel.Id);
                if (existingClient != null)
                {
                    using (DbConnection dbConnection = new DbConnection())
                    {
                        using (var connection = dbConnection.Connection)
                        {
                            DynamicParameters parameters = new DynamicParameters();
                            parameters.Add("@SubsiteId", existingClient.SubsiteId);
                            parameters.Add("@DisplayName", clientModel.Name);
                            parameters.Add("@Domain", clientModel.Domain);
                            parameters.Add("@Description", clientModel.Description);
                            parameters.Add("@Stopped", clientModel.Stopped);
                            parameters.Add("@EmailStopped", clientModel.EmailStopped);

                            result = connection.Execute("spUpdateSubsiteForClient", parameters, commandType: CommandType.StoredProcedure);
                        }
                    }

                    if (result == 0)
                    {
                        return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute() });
                    }

                    using (DbConnection dbConnection = new DbConnection())
                    {
                        using (var connection = dbConnection.Connection)
                        {
                            DynamicParameters parameters = new DynamicParameters();
                            parameters.Add("@ClientId", clientModel.Id);
                            parameters.Add("@Name", clientModel.Name);
                            parameters.Add("@SubsiteId", existingClient.SubsiteId);
                            parameters.Add("@Code", clientModel.Code);

                            result = connection.Execute("spUpdateClient", parameters, commandType: CommandType.StoredProcedure);
                        }
                    }
                    if (result == 0)
                    {
                        return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute() });
                    }
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataUpdatedSuccessfully, "Client") });
                }
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgIdNotFound, "Client") });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }

        /// <summary>
        /// This method returns client details of given client id
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetClientDetailById(int clientId)
        {
            if (clientId == null)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute() });
            }
            var clientExists = _CIRDbContext.Clients.Any(x => x.Id == clientId);
            if (!clientExists)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
            }
            try
            {
                ClientModel clientDetail;

                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@ClientId", clientId);
                        clientDetail = connection.QueryFirst<ClientModel>("spGetClientDetailById", parameters, commandType: CommandType.StoredProcedure);
                    }
                }
                return new JsonResult(new CustomResponse<ClientModel>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = clientDetail });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }

        /// <summary>
        /// This method is used to return all the clients along with its child portals
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<ClientSubModel> clients = new List<ClientSubModel>();
                List<PortalSubModel> portals = new List<PortalSubModel>();
                List<ClientPortalsSubModel> clientportals = new List<ClientPortalsSubModel>();
                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        clients = connection.Query<ClientSubModel>("spGetAllClients", null, commandType: CommandType.StoredProcedure).ToList();
                    }
                }
                foreach (var client in clients)
                {
                    using (DbConnection dbConnection = new DbConnection())
                    {
                        using (var connection = dbConnection.Connection)
                        {
                            DynamicParameters parameters = new DynamicParameters();
                            parameters.Add("@ClientId", client.Id);
                            portals = connection.Query<PortalSubModel>("spGetAllClientPortals", parameters, commandType: CommandType.StoredProcedure).ToList();
                            clientportals.Add(new ClientPortalsSubModel
                            {
                                ClientId = client.Id,
                                ClientName = client.Name,
                                Portals = portals
                            });
                        }
                    }
                }
                if (clientportals.Count == 0)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
                }
                return new JsonResult(new CustomResponse<List<ClientPortalsSubModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = clientportals });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }
        #endregion
    }
}
