using CIR.Core.Interfaces.Website;
using CIR.Core.ViewModel.Website;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.Website
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<IActionResult> CreateOrUpdateClient(ClientModel clientModel)
        {
            return await _clientRepository.CreateOrUpdateClient(clientModel);
        }

        public async Task<IActionResult> GetAll()
        {
            return await _clientRepository.GetAll();
        }

        public async Task<IActionResult> GetAllClients()
        {
            return await _clientRepository.GetAllClients();
        }

        public async Task<IActionResult> GetClientDetailById(int clientId)
        {
            return await _clientRepository.GetClientDetailById(clientId);
        }
    }
}
