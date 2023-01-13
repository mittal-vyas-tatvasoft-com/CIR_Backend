﻿using CIR.Core.ViewModel.Websites;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Websites
{
    public interface IClientService
    {
        Task<IActionResult> GetAllClients();
        Task<IActionResult> CreateClient(ClientModel clientModel);

    }
}
