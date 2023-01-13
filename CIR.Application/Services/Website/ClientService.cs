﻿using CIR.Core.Interfaces.Website;
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