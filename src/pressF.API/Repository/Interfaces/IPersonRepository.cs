using pressF.API.Authentication;
using pressF.API.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pressF.API.Repository.Interfaces
{
    public interface IPersonRepository : IRepository<Person>
    {
        public Task<InternalAuthResponse> Auth(string password, string login);
        public Task<bool> IsBlocked(string id);
        public Task<IEnumerable<Person>> New(DateTimeOffset date);
    }
}
