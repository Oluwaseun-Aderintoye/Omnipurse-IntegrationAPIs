using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using IntegrationAPIs.Models;

namespace IntegrationAPIs.Controllers
{
    public class ContactUsController : ApiController
    {
        OmnipurseDBEntities db = new OmnipurseDBEntities();
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/ContactUs/CreateContactUs")]
        public IHttpActionResult CreateContactUs([FromBody] ContactUs_Payload contactUs)
        {
            try
            {
                if(!string.IsNullOrEmpty(contactUs.EmailAddress) || !string.IsNullOrEmpty(contactUs.PhoneNumber))
                {
                    string email = contactUs.EmailAddress;
                    string phone = contactUs.PhoneNumber;
                    int personId = 0;
                    var objPerson = db.People.Where(obj => obj.EmailAddress == email || obj.EmailAddress2 == email || obj.PhoneHome == phone || obj.PhoneMobile == phone).FirstOrDefault();
                    if(objPerson != null)
                        personId = objPerson.PersonID;
                    else
                    {
                        // Create in Person table
                        var newPerson = new Person();
                        newPerson.PhoneMobile = phone;
                        newPerson.EmailAddress = email;
                        newPerson.FirstName = contactUs.Name;
                        newPerson.PersonTypeID = (int) Enums.PersonType.ContactUs;
                        newPerson.CreatedOnDate = DateTime.Now;
                        //newPerson.PhotoData = new byte[1];
                        db.People.Add(newPerson);
                        db.SaveChanges();
                        personId = newPerson.PersonID;
                    }
                    var objContactUs = new ContactU();
                    objContactUs.EmailAddress = email;
                    objContactUs.PhoneNumber = phone;
                    objContactUs.Name = contactUs.Name;
                    objContactUs.Subject = contactUs.Subject;
                    objContactUs.Message = contactUs.Message;
                    objContactUs.StatusID = (int) Enums.ContactUsStatus.Pending;
                    objContactUs.CreatedOnDate = DateTime.Now;
                    objContactUs.LastModifiedOnDate = DateTime.Now;
                    if (personId > 0)
                    {
                        objContactUs.PersonID = personId;
                        objContactUs.CreatedByWhoID = personId;
                        objContactUs.LastModifiedByWhoID = personId;
                    }
                    db.ContactUs.Add(objContactUs);
                    db.SaveChanges();

                    //#region Send Mail

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
                    //#endregion

                    //if logged in, pass PersonID to 
                    //objContactUs.CreatedByWhoID
                    //objContactUs.LastModifiedByWhoID
                }
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc.ToString());

                return Ok(new
                {
                    response = "failed; " + Request.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An internal error occurred: " + exc.ToString()
                )
                });
            }
            return Ok(new { response = "success" });
        }
                
        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}

public class ContactUs_Payload
{
    public string PhoneNumber { get; set; }
    public string EmailAddress { get; set; }
    public string Name { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
}

public class Newsletter_Payload
{
    //public string PhoneNumber { get; set; }
    public string EmailAddress { get; set; }
}
