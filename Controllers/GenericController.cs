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
using System.Web.Http.Cors;
using System.Net.Mail;

namespace IntegrationAPIs.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    
    public class GenericController : ApiController
    {
        OmnipurseDBEntities db = new OmnipurseDBEntities();

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Generic/GetCountrys")]
        public IHttpActionResult GetCountrys([FromBody] LogUserDTO logUserDTO)
        {
            IEnumerable<CountryDTO> listCountry = null; 
            try
            {
                listCountry = db.Countries
                  .Select(c => new CountryDTO { CountryID = c.CountryID, CountryName = c.CountryName })
                  .ToList();
            }
            catch (Exception exc)
            {
                #region Log Exception
                var logger = new DevUtility.Controllers.Logger();
                int levelType = (int)Enums.LevelType.Exception;
                int level = (int)Enums.Level.Error;
                string email = !string.IsNullOrEmpty(logUserDTO.LoggedUser)
                    ? logUserDTO.LoggedUser : null;
                logger.ExceptionLogger(exc, level, levelType, email);
                #endregion

                return Ok(new
                {
                    response = "failed"
                });
            }
            return Ok(new { response = "success", listCountry = listCountry });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Generic/GetStatesByCountryID")]
        public IHttpActionResult GetStatesByCountryID(int countryId, string loggedUser = "")
        {
            IEnumerable<StateDTO> listStates = null;
            try
            {
                listStates = db.States.Where(obj => obj.CountryID == countryId).Select(obj => new StateDTO { StateID = obj.StateID, CountryID = obj.CountryID, StateName = obj.StateName }).ToList();
            }
            catch (Exception exc)
            {
                #region Log Exception
                var logger = new DevUtility.Controllers.Logger();
                int levelType = (int)Enums.LevelType.Exception;
                int level = (int)Enums.Level.Error;
                string email = !string.IsNullOrEmpty(loggedUser)
                    ? loggedUser : null;
                logger.ExceptionLogger(exc, level, levelType);
                #endregion

                return Ok(new
                {
                    response = "failed"
                });
            }
            return Ok(new { response = "success", listStates = listStates });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Generic/TextConversion")]
        public IHttpActionResult TextConversion(string tt, string tv)
        {
            string outputValue = string.Empty;
            try
            {
                if(tt == "cipher")
                {
                    if(!string.IsNullOrEmpty(tv))
                    {
                        outputValue = Encryption.AesDecrypt(tv);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(tv))
                    {
                        outputValue = Encryption.AesEncrypt(tv);
                    }
                }
            }
            catch (Exception exc)
            {
                return Ok("failed");
            }
            return Ok(outputValue);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Generic/GetServices")]
        public IHttpActionResult GetServices()
        { 
            try
            {
                var listService = db.Services
                .Select(obj => new
                {
                    obj.ServiceID,
                    obj.ServiceName
                })
                .ToList();

                return Ok(new
                {
                    response = "success",
                    listService
                });
            }
            catch (Exception exc)
            {
                return Ok(new
                {
                    response = "failed"
                });
            }
        }

        public Dictionary<string, string> LoadSmtpSettings()
        {
            var settings = db.AppSettings
                .Where(obj => obj.SettingKey != null)
                .ToDictionary(
                    obj => obj.SettingKey,
                    obj => obj.SettingValue
                );

            return settings;
        }

        public string SendMail(string toEmail, string subject, string body)
        {
            try
            {
                var settings = LoadSmtpSettings();
                string encSmtpPassword = settings["SmtpPassword"];
                string smtpPassword = Encryption.AesDecrypt(encSmtpPassword);
                var smtpClient = new SmtpClient(settings["SmtpHost"], int.Parse(settings["SmtpPort"]))
                {
                    Credentials = new NetworkCredential(settings["SmtpUser"], smtpPassword),
                    EnableSsl = bool.Parse(settings["EnableSsl"])
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(settings["SenderEmail"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                smtpClient.Send(mailMessage);

                return "Email sent successfully.";
            }
            catch (Exception exc)
            {

                return "Failed to send email: " + exc.Message;
            }
        }
    }
}
