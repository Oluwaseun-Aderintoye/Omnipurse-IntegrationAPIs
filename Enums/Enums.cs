using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Enums
{
    public class Enums
    {
    }
    public enum StatusCode
    {
        Success = 200,
        BadRequest = 400,
        Unauthorized = 401,
        NotFound = 404,
        InternalError = 500
    }

    public enum ContactUsStatus
    {
        Pending = 2,
        InProgress = 4,
        Assigned = 6,
        Treated = 5
    }

    public enum PersonType
    {
        SuperAdmin = 1,
        Admin = 2,
        ContactUs = 3,
        Subscriber = 4,
        User = 5
    }

    public enum Environment
    {
        Development = 1,
        Testing = 2,
        Staging = 3,
        Production = 4,
        QA = 5
    }

    public enum LevelType
    {
        Exception = 1
    }

    public enum Level
    {
        Info = 1,
        Warning = 2,
        Error = 3,
        Critical = 4
    }
}