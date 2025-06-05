using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.classes
{
    public class Auth
    {

    }

    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserRegDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public string Password { get; set; }
    }
}