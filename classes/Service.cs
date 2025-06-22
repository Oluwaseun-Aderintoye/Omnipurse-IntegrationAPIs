using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.classes
{
    public class Service
    {
        
    }

    public class ConsultationDTO
    {
        public string Name { get; set;}
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CompanyName { get; set; }
        public int ServiceID { get; set; }
        public string ScheduleDate { get; set; }
        public string ScheduleTime { get; set; }
        public string AdditionalInfo { get; set; }
        public string LoggedUser { get; set; }
    }

}