using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GreaterHeight.DevMessaging;
using GreaterHeight.DevUtil;
using IntegrationAPIs.Enums;
using IntegrationAPIs.Models;
using DevUtility.Controllers;
using IntegrationAPIs.classes;

namespace IntegrationAPIs.Controllers
{
    public class ConsultationController : ApiController
    {
        OmnipurseDBEntities db = new OmnipurseDBEntities();

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Consultation/BookConsultation")]
        public IHttpActionResult BookConsultation(ConsultationDTO consultationDTO)
        {
            try
            {
                #region Create Consultation
                Consultation objConsultation = new Consultation();
                objConsultation.Name = consultationDTO.Name;
                objConsultation.EmailAddress = consultationDTO.Email;
                objConsultation.PhoneNumber = consultationDTO.Phone;
                objConsultation.CompanyName = consultationDTO.CompanyName;
                objConsultation.ServiceID = consultationDTO.ServiceID;
                objConsultation.ScheduleDate = Convert.ToDateTime(consultationDTO.ScheduleDate);
                TimeSpan scheduleTime = TimeSpan.Parse(consultationDTO.ScheduleTime);
                objConsultation.ScheduleTime = scheduleTime;
                objConsultation.StatusID = 2; //PENDING
                objConsultation.AdditionalInformation = consultationDTO.AdditionalInfo;
                objConsultation.CreatedOnDate = DateTime.Now;
                if(!string.IsNullOrEmpty(consultationDTO.LoggedUser))
                objConsultation.CreatedByUserID = consultationDTO.LoggedUser;

                db.Consultations.Add(objConsultation);
                db.SaveChanges();
                #endregion

                #region Send Mail Receipt
                var generic = new GenericController();
                string toMail = consultationDTO.Email;
                var appSettings = generic.LoadSmtpSettings();
                string mailSubject = appSettings["ConsultationMailSubject"];

                string serviceName = string.Empty;
                if(consultationDTO.ServiceID > 0)
                {
                    int serviceId = consultationDTO.ServiceID;
                    var objService = db.Services.Where(obj => obj.ServiceID == serviceId).FirstOrDefault();
                    if(objService != null)
                    {
                        serviceName = objService.ServiceName;
                    }    
                }

                string mailBody = appSettings["ConsultationMailBody"];
                mailBody = mailBody.Replace("[UserName]", consultationDTO.Name)
                    .Replace("[ServiceArea]", serviceName)
                    .Replace("[ScheduleDate]", consultationDTO.ScheduleDate)
                    .Replace("[ScheduleTime]", consultationDTO.ScheduleTime)
                    .Replace("[UserMessage]", consultationDTO.AdditionalInfo);
                generic.SendMail(toMail, mailSubject, mailBody);
                #endregion
               
            }
            catch (Exception exc)
            {
                #region Log Exception
                var logger = new DevUtility.Controllers.Logger();
                int levelType = (int)Enums.LevelType.Exception;
                int level = (int)Enums.Level.Error;
                string email = !string.IsNullOrEmpty(consultationDTO.Email)
                    ? consultationDTO.Email : null;
                logger.ExceptionLogger(exc, level, levelType, email);
                #endregion

                return Ok(new
                {
                    response = "failed"
                });
            }
            return Ok(new { response = "success"});
        }


    }
}
