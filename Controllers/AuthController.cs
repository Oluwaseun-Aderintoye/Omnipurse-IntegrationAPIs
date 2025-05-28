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
    public class AuthController : ApiController
    {
        OmnipurseDBEntities db = new OmnipurseDBEntities();
        
        [AllowAnonymous]
        [HttpPost]
        [Route("api/Auth/Login")]
        public IHttpActionResult Login([FromBody] Login login)
        {
            bool isUserMatch = false;
            try
            {
                string email = login.Email;
                string pass = login.Password;
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(pass))
                {
                    //confirm valid email
                    bool emailValid =  Mailer.IsMailAddressCorrectSyntax(email);
                    if(!emailValid)
                        return Ok(new
                        {
                            response = "email invalid"
                        });

                    string cipherPass = Encryption.AesEncrypt(pass);
                    var objUser = db.Users.Where(obj => obj.Email == email && obj.PasswordHash == cipherPass).FirstOrDefault();
                    if(objUser != null)
                    {
                        isUserMatch = true;
                    }
                }
            }
            catch (Exception exc)
            {
                #region Log Exception
                var logger = new DevUtility.Controllers.Logger();
                int levelType = (int)Enums.LevelType.Exception;
                int level = (int)Enums.Level.Error;
                string email = !string.IsNullOrEmpty(login.Email)
                    ? login.Email : null;
                logger.ExceptionLogger(exc, level, levelType, email);
                #endregion

                return Ok(new
                {
                    response = "failed"
                });
            }
            return Ok(new { response = "success", isUserMatch = isUserMatch });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Auth/CreateAccount")]
        public IHttpActionResult CreateAccount([FromBody] Login login)
        {
            bool isUserMatch = false;
            try
            {
                string email = login.Email;
                string pass = login.Password;
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(pass))
                {
                    //confirm valid email
                    bool emailValid = Mailer.IsMailAddressCorrectSyntax(email);
                    if (!emailValid)
                        return Ok(new
                        {
                            response = "email invalid"
                        });

                    string cipherPass = Encryption.AesEncrypt(pass);
                    var objUser = db.Users.Where(obj => obj.Email == email && obj.PasswordHash == cipherPass).FirstOrDefault();
                    if (objUser != null)
                    {
                        isUserMatch = true;
                    }
                }
            }
            catch (Exception exc)
            {
                #region Log Exception
                var logger = new DevUtility.Controllers.Logger();
                int levelType = (int)Enums.LevelType.Exception;
                int level = (int)Enums.Level.Error;
                string email = !string.IsNullOrEmpty(login.Email)
                    ? login.Email : null;
                logger.ExceptionLogger(exc, level, levelType, email);
                #endregion

                return Ok(new
                {
                    response = "failed"
                });
            }
            return Ok(new { response = "success", isUserMatch = isUserMatch });
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("api/Auth/RetrievePassword")]
        public IHttpActionResult RetrievePassword([FromBody] EmailAddress emailAddress)
        {
            string plainText = string.Empty;
            try
            {
                string email = emailAddress.Email;
                if (!string.IsNullOrEmpty(email))
                {
                    var objUser = db.Users.Where(obj => obj.Email == email).FirstOrDefault();
                    if (objUser != null)
                    {
                        string cipherText = objUser.PasswordHash;
                        if (!string.IsNullOrEmpty(cipherText))
                        {
                            plainText = Encryption.AesDecrypt(cipherText);
                        }
                    }
                }

            }
            catch (Exception exc)
            {
                #region Log Exception
                var logger = new DevUtility.Controllers.Logger();
                int levelType = (int)Enums.LevelType.Exception;
                int level = (int)Enums.Level.Error;
                string email = !string.IsNullOrEmpty(emailAddress.Email)
                    ? emailAddress.Email : null;
                logger.ExceptionLogger(exc, level, levelType, email);
                #endregion

                return Ok(new
                {
                    response = "failed"
                });
            }
            return Ok(new { response = "success", plainText = plainText });
        }

    }
}
