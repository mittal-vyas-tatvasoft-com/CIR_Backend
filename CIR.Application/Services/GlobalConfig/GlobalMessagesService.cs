using CIR.Core.Entities.Users;
using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.Interfaces.Users;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Application.Services.GlobalConfig
{
	public class GlobalMessagesService : IGlobalMessagesService
	{
		private readonly IGlobalMessagesRepository _globalMessageRepository;

		public GlobalMessagesService(IGlobalMessagesRepository globalMessageRepository)
		{
			_globalMessageRepository = globalMessageRepository;
		}

		public async Task<IActionResult> GetGlobalMessagesList(int cultureID)
		{
			return await _globalMessageRepository.GetGlobalMessagesList(cultureID);
		}
		public async Task<IActionResult> CreateOrUpdateGlobalMessages(List<GlobalMessagesModel> globalMessageModel)
		{
			return await _globalMessageRepository.CreateOrUpdateGlobalMessages(globalMessageModel);
		}
	}
}
