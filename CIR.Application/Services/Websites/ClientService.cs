using CIR.Core.Interfaces.Websites;
using CIR.Core.ViewModel.Websites;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.Websites
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<IActionResult> CreateClient(ClientModel clientModel)
        {
            return await _clientRepository.CreateClient(clientModel);
        }

        public async Task<IActionResult> GetAllClients()
        {
            return await _clientRepository.GetAllClients();
        }
    }
}
