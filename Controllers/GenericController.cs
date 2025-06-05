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

        

    }
}
