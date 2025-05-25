using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using DevUtility.Models;
using Newtonsoft.Json;

namespace IntegrationAPIs.Controllers
{
    public class UtilityController : ApiController
    {
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

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        public void WriteExceptionLog(Exception exc,int levelId, int levelTypeId, int? userId, int? personId)
        {
            #region Log Exception
            var logger = new DevUtility.Controllers.Logger();
            var stackTrace = new StackTrace(exc, true);
            var frame = stackTrace.GetFrames()?.FirstOrDefault(f => f.GetFileLineNumber() > 0);

            int environmentId = 0;
            var homeController = new HomeController();
            string hostUrl = homeController.GetRootUrl();
            environmentId = (hostUrl.Contains("localhost"))
                ? (int)Enums.Environment.Development
                : (int)Enums.Environment.Production;

            var log = new ExceptionLog
            {
                Timestamp = DateTime.UtcNow,
                LevelID = levelId,
                LevelTypeID = levelTypeId,
                Message = exc.Message,
                ExceptionType = exc.GetType().FullName,
                StackTrace = exc.StackTrace,
                Source = exc.Source,
                MethodName = frame?.GetMethod()?.Name,
                LineNumber = frame?.GetFileLineNumber() ?? 0,
                RequestPath = HttpContext.Current?.Request?.Path,
                HttpMethod = HttpContext.Current?.Request?.HttpMethod,
                UserID = userId, //Convert.ToInt32(User.Identity..GetUserId()),
                PersonID = personId,
                AdditionalData = JsonConvert.SerializeObject(new
                {
                    Headers = HttpContext.Current.Request.Headers,
                    Form = HttpContext.Current.Request.Form
                }),
                CreatedOnDate = DateTime.UtcNow,
                CorrelationID = HttpContext.Current?.Items["CorrelationID"]?.ToString(),
                ClientIp = HttpContext.Current?.Request?.UserHostAddress,
                UserAgent = HttpContext.Current?.Request?.UserAgent,
                EnvironmentID = environmentId
            };

            #endregion
        }

    }
}
