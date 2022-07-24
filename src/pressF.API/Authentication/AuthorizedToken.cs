using pressF.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace pressF.API.Authentication
{
    public class AuthorizedToken
    {
        public Person Person { get; set; }
        public string Token { get; set; }
    }
}
