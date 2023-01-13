using CIR.Core.ViewModel.Website;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Website
{
    public interface IClientRepository
    {
        Task<IActionResult> GetAllClients();
        Task<IActionResult> CreateClient(ClientModel clientModel);
    }
}
