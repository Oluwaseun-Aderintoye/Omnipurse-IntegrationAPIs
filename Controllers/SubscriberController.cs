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

namespace IntegrationAPIs.Controllers
{
    public class SubscriberController : ApiController
    {
        OmnipurseDBEntities db = new OmnipurseDBEntities();
        
        [AllowAnonymous]
        [HttpPost]
        [Route("api/Subscriber/SubscribeToNewsletter")]
        public IHttpActionResult SubscribeToNewsletter([FromBody] Newsletter_Payload newsletter)
        {
            try
            {
                if (!string.IsNullOrEmpty(newsletter.EmailAddress))
                {
                    string email = newsletter.EmailAddress;
                    //string phone = newsletter.PhoneNumber;

                    //check if email is valid
                    bool isEmailValidSyntax = Mailer.IsMailAddressCorrectSyntax(email);
                    if(!isEmailValidSyntax)
                        return Ok(new { response = "invalid email syntax" });
                    string pass = string.Empty;
                    int personId = 0;
                    var objPerson = db.People.Where(obj => obj.EmailAddress == email || obj.EmailAddress2 == email).FirstOrDefault();
                    if (objPerson != null)
                        personId = objPerson.PersonID;
                    else
                    {
                        #region Create User
                        //Create in User table
                        var objUser = new User();
                        objUser.Email = email;
                        //Generate a pass                
                        pass = DevUtility.Controllers.Utility.GenerateRandomNumberByLength(6);
                        string encryptedPass = Encryption.AesEncrypt(pass.ToString());
                        objUser.PasswordHash = encryptedPass;
                        objUser.Username = pass;
                        objUser.CreatedOnDate = DateTime.Now;
                        db.Users.Add(objUser);
                        db.SaveChanges();
                        int userId = objUser.UserID;
                        #endregion

                        #region Create Person
                        // Create in Person table
                        var newPerson = new Person();
                        //if (!string.IsNullOrEmpty(phone))
                        //    newPerson.PhoneMobile = phone;
                        newPerson.EmailAddress = email;
                        newPerson.PersonTypeID = (int)Enums.PersonType.Subscriber;
                        newPerson.UserID = userId;
                        newPerson.CreatedOnDate = DateTime.Now;
                        db.People.Add(newPerson);
                        db.SaveChanges();
                        personId = newPerson.PersonID;
                        
                        #endregion
                    }

                        #region Create Person Type
                    int personTypeId = (int)Enums.PersonType.Subscriber;
                    var objActiveSubscriber = db.PersonPersonTypes.Where(obj => obj.PersonID == personId && obj.PersonTypeID == personTypeId).FirstOrDefault();
                    if(objActiveSubscriber == null)
                    {
                        var objPPT = new PersonPersonType();
                        objPPT.PersonTypeID = personTypeId;
                        objPPT.CreatedOnDate = DateTime.Now;
                        objPPT.LastModifiedOnDate = DateTime.Now;
                        if (personId > 0)
                        {
                            objPPT.PersonID = personId;
                            //objPPT.CreatedByUserID = personId;
                            //objPPT.LastModifiedByUserID = personId;
                        }
                        db.PersonPersonTypes.Add(objPPT);
                        db.SaveChanges();
                        #endregion

                        #region Send Mail
                        //Mailer.SendFormattedHtmlEmail(email, "subscribe", pass.ToString());
                        //var utilService = new Services.UtilService();
                        //var appSetting = db.AppSettings.Where(obj => obj.SettingKey == "ContactUsMailReceipt").FirstOrDefault();
                        //if(appSetting != null)
                        //{
                        //    string mailBody = appSetting.SettingValue;
                        //    mailBody = mailBody.Replace("[UserName]", contactUs.Name)
                        //        .Replace("[UserEmail]", contactUs.EmailAddress)
                        //        .Replace("[UserSubject]", contactUs.Subject)
                        //        .Replace("[UserMessage]", contactUs.Message);

                        //    utilService.SendFormattedHTMLEmail(contactUs.EmailAddress, contactUs.Subject, mailBody);
                        //}
                        #endregion

                        //if logged in, pass PersonID to 
                        //objContactUs.CreatedByWhoID
                        //objContactUs.LastModifiedByWhoID
                    }
                    else
                        return Ok(new { response = "already subscribed" });
                }
                
            }
            catch (Exception exc)
            {
                #region Log Exception
                var logger = new DevUtility.Controllers.Logger();
                int levelType = (int)Enums.LevelType.Exception;
                int level = (int)Enums.Level.Error;
                string email = !string.IsNullOrEmpty(newsletter.EmailAddress)
                    ? newsletter.EmailAddress : null;
                logger.ExceptionLogger(exc, level, levelType, email);
                #endregion

                return Ok(new
                {
                    response = "failed"
                });
            }
            return Ok(new { response = "success" });
        }
        
    }
}
