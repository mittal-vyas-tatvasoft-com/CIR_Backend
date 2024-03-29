﻿using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Interfaces.Website;
using CIR.Core.ViewModel.Website;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.Website
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PortalsController : ControllerBase
    {
        #region PROPERTIES

        private readonly IPortalService _portalService;

        #endregion


        #region CONSTRUCTORS

        public PortalsController(IPortalService portalService)
        {
            _portalService = portalService;
        }

        #endregion


        #region METHODS

        /// <summary>
        /// This method takes all the portal details and adds it
        /// </summary>
        /// <param name="portalModel"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpPost("Create/{clientId}")]
        public async Task<IActionResult> Create(PortalModel portalModel, long clientId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await _portalService.CreatePortal(portalModel, clientId);
                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
                }
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });
        }

        /// <summary>
        /// This method disbles the portal of given portal Id
        /// </summary>
        /// <param name="portalId"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{portalId}")]
        public async Task<IActionResult> Delete(long portalId)
        {
            try
            {
                return await _portalService.DisablePortal(portalId);
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }

        /// <summary>
        /// This method takes portal details and updates it
        /// </summary>
        /// <param name="portalModel"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public async Task<IActionResult> Update(PortalModel portalModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await _portalService.UpdatePortal(portalModel);
                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
                }
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });
        }

        /// <summary>
        /// This method will return all the portals under given client
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet("clientId")]
        public async Task<IActionResult> GetById(int clientId)
        {
            try
            {
                return await _portalService.GetByClientId(clientId);
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }

        /// <summary>
        /// This method will return portal details of given portal id
        /// </summary>
        /// <param name="portalId"></param>
        /// <returns></returns>
        [HttpGet("Id/{portalId}")]
        public async Task<IActionResult> GetDetailByid(int portalId)
        {
            try
            {
                return await _portalService.GetById(portalId);
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }

        /// <summary>
        /// This portal will make clone a new portal and return its Id
        /// </summary>
        /// <param name="portalId"></param>
        /// <param name="cultureId"></param>
        /// <param name="destionationDirectory"></param>
        /// <param name="destinationClientId"></param>
        /// <returns></returns>
        [HttpPost("Clone")]
        public async Task<IActionResult> Clone(long portalId, long cultureId, string destionationDirectory, long destinationClientId)
        {
            try
            {
                return await _portalService.ClonePortal(portalId, cultureId, destionationDirectory, destinationClientId);
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }

        #endregion
    }
}
