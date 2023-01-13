using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Entities.Users;
using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Data.Data.GlobalConfig
{
    public class GlobalConfigEmailRepository : IGlobalEmailRepository
    {
        private readonly CIRDbContext _CIRDBContext;
        public GlobalConfigEmailRepository(CIRDbContext context)
        {
            _CIRDBContext = context ??
                throw new ArgumentNullException(nameof(context));
        }
        public async Task<IActionResult> GetglobalEmailModelById(int id)
        {
            var result = (from globalMessages in _CIRDBContext.GlobalConfigurationEmails
                          select new GlobalConfigurationEmailsGetModel()
                          {
                              Id = globalMessages.Id,
                              CultureId = globalMessages.CultureId,
                              FieldTypeId = globalMessages.FieldTypeId,
                              Content = globalMessages.Content,
                              Subject = globalMessages.Subject,

                          }).ToList();

            var emailId = await _CIRDBContext.GlobalConfigurationEmails.Where(x => x.Id == id).FirstOrDefaultAsync();
            var serializedParent = JsonConvert.SerializeObject(emailId);
            GlobalConfigurationEmailsGetModel email = JsonConvert.DeserializeObject<GlobalConfigurationEmailsGetModel>(serializedParent);

            if (email.Content.Contains("[Reference])") || email.Subject.Contains("[Reference])"))
            {
                email.Reference = true;
            }
            if (email.Content.Contains("[BookingId])") || email.Subject.Contains("[BookingId])"))
            {
                email.BookingId = true;
            }
            if (email.Content.Contains("[OrderNumber])") || email.Subject.Contains("[OrderNumber])"))
            {
                email.OrderNumber = true;
            }
            if (email.Content.Contains("[CustomerEmail])") || email.Subject.Contains("[CustomerEmail])"))
            {
                email.CustomerEmail = true;
            }
            if (email.Content.Contains("[CustomerName])") || email.Subject.Contains("[CustomerName])"))
            {
                email.CustomerName = true;
            }
            if (email.Content.Contains("[CollectionAddress])") || email.Subject.Contains("[CollectionAddress])"))
            {
                email.CollectionAddress = true;
            }
            if (email.Content.Contains("[TrackingURL])") || email.Subject.Contains("[TrackingURL])"))
            {
                email.TrackingURL = true;
            }
            if (email.Content.Contains("[LabelURL])") || email.Subject.Contains("[LabelURL])"))
            {
                email.LabelURL = true;
            }
            if (email.Content.Contains("[BookingURL])") || email.Subject.Contains("[BookingURL])"))
            {
                email.BookingURL = true;
            }
            if (result != null)
                return new JsonResult(new CustomResponse<List<GlobalConfigurationEmailsGetModel>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = result });
            else
                return new JsonResult(new CustomResponse<List<GlobalConfigurationEmailsGetModel>>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound });

        }
        public async Task<IActionResult> CreateOrUpdateGlobalEmail(List<GlobalConfigurationEmailsModel> globalEmailModel)
        {
            if (globalEmailModel.Any(x => x.Id == 0))
            {
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest });

            }
            if (globalEmailModel != null)
            {
                foreach (var item in globalEmailModel)
                {
                    if (item.Id != 0)
                    {
                        var emailsCheck = _CIRDBContext.GlobalConfigurationEmails.FirstOrDefault(x => x.Id == item.Id);
                        if (emailsCheck != null)
                        {
                            emailsCheck.FieldTypeId = item.FieldTypeId;
                            emailsCheck.CultureId = item.CultureId;
                            emailsCheck.Content = item.Content;
                            emailsCheck.Subject = item.Subject;
                        }
                        else
                        {
                            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest });
                        }
                    }
                    else
                    {
                        GlobalConfigurationEmails email = new GlobalConfigurationEmails()
                        {
                            Id = item.Id,
                            FieldTypeId = item.FieldTypeId,
                            CultureId = item.CultureId,
                            Content = item.Content,
                            Subject = item.Subject
                        };
                        _CIRDBContext.GlobalConfigurationEmails.Add(email);
                    }
                }
                _CIRDBContext.SaveChanges();
                return new JsonResult(new CustomResponse<List<GlobalConfigurationEmails>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success });
            }
            return new JsonResult(new CustomResponse<GlobalConfigurationEmails>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound });
        }
    }
}

