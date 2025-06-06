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
        public IHttpActionResult Login([FromBody] LoginDTO loginDTO)
        {
            bool isUserMatch = false;   
            try
            {
                string email = loginDTO.Email;
                string pass = loginDTO.Password;
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
                string email = !string.IsNullOrEmpty(loginDTO.Email)
                    ? loginDTO.Email : null;
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
        public IHttpActionResult CreateAccount([FromBody] UserRegDto userRegDto)
        {
            try
            {
                #region Validation

                string email = userRegDto.EmailAddress;
                bool isValidMail = Mailer.IsMailAddressCorrectSyntax(email);
                if(!isValidMail)
                    return Ok( new { response = "invalid email"});
                
                #endregion
                #region Create User
                var objUser = new User();
                objUser.Email = userRegDto.EmailAddress;
                string encryptedPass = Encryption.AesEncrypt(userRegDto.Password);
                objUser.PasswordHash = encryptedPass;
                objUser.PhoneNumber = userRegDto.PhoneNumber;
                objUser.IsPasswordUserDefined = true;
                objUser.IsActive = true;
                objUser.CreatedOnDate = DateTime.Now;
                db.Users.Add(objUser);
                db.SaveChanges();
                int userId = objUser.UserID;
                #endregion

                #region Create Address
                var objAddress = new Address();
                objAddress.CountryID = userRegDto.CountryId;
                objAddress.StateID = userRegDto.StateId;
                objAddress.CreatedOnDate = DateTime.Now;
                objAddress.UserID = userId;
                db.Addresses.Add(objAddress);
                db.SaveChanges();
                #endregion

                #region Create Person
                var newPerson = new Models.Person();
                newPerson.EmailAddress = userRegDto.EmailAddress;
                newPerson.UserID = userId;
                if(!string.IsNullOrEmpty(userRegDto.CompanyName))
                {
                    newPerson.CompanyName = userRegDto.CompanyName;
                    newPerson.IsCompany = true;
                }
                else
                {
                    newPerson.FirstName = userRegDto.FirstName;
                    newPerson.LastName = userRegDto.LastName;
                }
                newPerson.PhoneMobile = userRegDto.PhoneNumber;
                newPerson.CreatedOnDate = DateTime.Now;
                db.People.Add(newPerson);
                db.SaveChanges();
                int personId = newPerson.PersonID;
                #endregion
                #region create PersonType
                var objPPT = new PersonPersonType();
                objPPT.PersonID = personId;
                objPPT.PersonTypeID = (int) Enums.PersonType.User;
                objPPT.CreatedOnDate = DateTime.Now;
                objPPT.CreatedByUserID = userId;
                db.PersonPersonTypes.Add(objPPT);
                db.SaveChanges();
                #endregion
            }
            catch (Exception exc)
            {
                #region Log Exception
                var logger = new DevUtility.Controllers.Logger();
                int levelType = (int)Enums.LevelType.Exception;
                int level = (int)Enums.Level.Error;
                string email = !string.IsNullOrEmpty(userRegDto.EmailAddress)
                    ? userRegDto.EmailAddress : null;
                logger.ExceptionLogger(exc, level, levelType, email);
                #endregion

                return Ok(new
                {
                    response = "failed"
                });
            }
            return Ok(new { response = "success" });
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("api/Auth/RetrievePassword")]
        public IHttpActionResult RetrievePassword([FromBody] EmailDTO emailDTO)
        {
            string plainText = string.Empty;
            try
            {
                string email = emailDTO.Email;
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
                string email = !string.IsNullOrEmpty(emailDTO.Email)
                    ? emailDTO.Email : null;
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
