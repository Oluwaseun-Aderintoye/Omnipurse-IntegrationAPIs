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
    public class GenericController : ApiController
    {
        OmnipurseDBEntities db = new OmnipurseDBEntities();
        
        [AllowAnonymous]
        [HttpPost]
        [Route("api/Generic/GetCountrys")]
        public IHttpActionResult GetCountrys([FromBody] LogUser loggedUser)
        {
            IEnumerable<Country> listCountry = null; 
            try
            {
                listCountry = db.Countries.ToList();
            }
            catch (Exception exc)
            {
                #region Log Exception
                var logger = new DevUtility.Controllers.Logger();
                int levelType = (int)Enums.LevelType.Exception;
                int level = (int)Enums.Level.Error;
                string email = !string.IsNullOrEmpty(loggedUser.LoggedUser)
                    ? loggedUser.LoggedUser : null;
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
        [Route("api/Generic/GetStatesByCountryID/{countryId}/{loggedUser}")]
        public IHttpActionResult GetStatesByCountryID(int countryId, string loggedUser = "")
        {
            IEnumerable<State> listStates = null;
            try
            {
                listStates = db.States.Where(obj => obj.CountryID == countryId).ToList();
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
