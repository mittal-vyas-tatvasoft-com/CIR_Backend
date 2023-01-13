using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities;
using CIR.Core.Entities.Website;
using CIR.Core.Interfaces.Website;
using CIR.Core.ViewModel.Website;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        /// This method will be used by create method of clients controller
        /// </summary>
        /// <param name="clientModel"></param>
        /// <returns>return Ok if successful else returns bad request</returns>
        public async Task<IActionResult> CreateClient(ClientModel clientModel)
        {
            try
            {
                if (clientModel != null)
                {
                    if (clientModel.Id < 0 || clientModel.SubsiteId < 0)
                    {
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Error occurred while adding new client" });
                    }
                    var subsite = new SubSite()
                    {
                        DisplayName = clientModel.Name,
                        Domain = clientModel.Domain,
                        Description = clientModel.Description,
                        Stopped = clientModel.Stopped,
                        EmailStopped = clientModel.EmailStopped,
                        ShowTax = false,
                        Enabled = false
                    };
                    _CIRDbContext.SubSites.Add(subsite);
                    _CIRDbContext.SaveChanges();

                    var subsiteId = subsite.Id;

                    var client = new Clients()
                    {
                        Id = clientModel.Id,
                        SubsiteId = subsiteId,
                        Name = clientModel.Name,
                        Code = clientModel.Code
                    };
                    _CIRDbContext.Clients.Add(client);
                    _CIRDbContext.SaveChanges();

                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = "Client Added Successfully" });

                }
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Error occurred while adding new client" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        /// <summary>
        /// this method will be used by GetAll method of clients controller
        /// </summary>
        /// <returns>returns list of all the clients</returns>
        public async Task<IActionResult> GetAllClients()
        {
            try
            {
                var clients = await (from client in _CIRDbContext.Clients
                                     join subsite in _CIRDbContext.SubSites
                                     on client.SubsiteId equals subsite.Id
                                     select new ClientModel()
                                     {
                                         Id = client.Id,
                                         Name = client.Name,
                                         SubsiteId = subsite.Id,
                                         Code = client.Code,
                                         Domain = subsite.Domain,
                                         Description = subsite.Description,
                                         Stopped = subsite.Stopped,
                                         EmailStopped = subsite.EmailStopped
                                     }).ToListAsync();

                if (clients.Count > 0)
                {
                    return new JsonResult(new CustomResponse<List<ClientModel>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = clients });
                }
                else
                {
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.NoContent, Result = true, Message = HttpStatusCodesMessages.NoContent, Data = "No Data is present" });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }
        #endregion
    }
}
