using pressF.API.Enums;
using pressF.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pressF.API.Authentication
{
    public class InternalAuthResponse
    {
        public Person AuthoredPerson { get; set; }
        public StatusAuthResponse Status { get; set; }
        public string Message { get; set; }
    }
}
