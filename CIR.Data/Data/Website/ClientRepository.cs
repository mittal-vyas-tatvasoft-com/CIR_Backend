using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities;
using CIR.Core.Entities.GlobalConfiguration;
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
				await _CIRDbContext.SaveChangesAsync();

				var client = new Clients()
				{
					Id = clientModel.Id,
					SubsiteId = subsite.Id,
					Name = clientModel.Name,
					Code = clientModel.Code
				};
				_CIRDbContext.Clients.Add(client);
				await _CIRDbContext.SaveChangesAsync();
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
				var existingClient = _CIRDbContext.Clients.FirstOrDefault(client => client.Id == clientModel.Id);
				if (existingClient != null)
				{
					var subsite = new SubSite()
					{
						Id = existingClient.SubsiteId,
						DisplayName = clientModel.Name,
						Domain = clientModel.Domain,
						Description = clientModel.Description,
						Stopped = clientModel.Stopped,
						EmailStopped = clientModel.EmailStopped,
						ShowTax = false,
						Enabled = false
					};
					_CIRDbContext.SubSites.Update(subsite);
					await _CIRDbContext.SaveChangesAsync();

					_CIRDbContext.Clients.Where(_ => _.Id == clientModel.Id).ToList().ForEach((client =>
					{
						client.Name = clientModel.Name;
						client.SubsiteId = subsite.Id;
						client.Code = clientModel.Code;
					}
					));
					await _CIRDbContext.SaveChangesAsync();
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataUpdatedSuccessfully, "Client") });
				}
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgIdNotFound, "Client") });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}
		#endregion
	}
}
