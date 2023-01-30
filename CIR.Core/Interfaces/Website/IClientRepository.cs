using CIR.Core.ViewModel.Website;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Website
{
    public interface IClientRepository
    {
        Task<IActionResult> GetAllClients();
        Task<IActionResult> CreateOrUpdateClient(ClientModel clientModel);
        Task<IActionResult> GetClientDetailById(int clientId);
        Task<IActionResult> GetAll();
    }
}
