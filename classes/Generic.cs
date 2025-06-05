using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.classes
{
    public class Generic
    {
        
    }

    public class EmailDTO
    {
        public string Email { get; set;}
    }

    public class LogUserDTO
    {
        public string LoggedUser { get; set; }
    }

    public class CountryDTO
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
    }

    public class StateDTO
    {
        public int StateID { get; set; }
        public int? CountryID { get; set; }
        public string StateName { get; set; }
    }
}