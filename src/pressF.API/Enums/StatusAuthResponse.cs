using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pressF.API.Enums
{
    public enum StatusAuthResponse
    {
        Error,
        Authorized,        
        Excluded,
        NotFound
    }
}
