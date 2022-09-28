using MongoDB.Driver;
using pressF.API.Authentication;
using pressF.API.Enums;
using pressF.API.Model;
using pressF.API.Repository.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace pressF.API.Repository
{
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        public PersonRepository(IMongoContext context) : base(context)
        {
        }

        public async Task<InternalAuthResponse> Auth(string password, string login)
        {
            try
            {
                var query = await DbSet.FindAsync(Builders<Person>.Filter.Eq("Password", password) & Builders<Person>.Filter.Eq("Username", login));
                var data = query.ToList();

                if (data == null || data.Count == 0)
                    return new InternalAuthResponse { AuthoredPerson = null, Status = StatusAuthResponse.NotFound, Message = "User not found." };
                if (data.Count == 1 && data.FirstOrDefault().Excluded == false)
                    return new InternalAuthResponse { AuthoredPerson = data.FirstOrDefault(), Status = StatusAuthResponse.Authorized, Message = "Authorized." };
                if (data.Count == 1 && data.FirstOrDefault().Excluded)
                    return new InternalAuthResponse { AuthoredPerson = null, Status = StatusAuthResponse.Excluded, Message = "User has no permission to get authorized." };
                if (data.Count > 1)
                    return new InternalAuthResponse { AuthoredPerson = null, Status = StatusAuthResponse.Error, Message = "There is more than one user with same username and password." };

                return new InternalAuthResponse { AuthoredPerson = null, Status = StatusAuthResponse.NotFound, Message = "User not found." };
            }
            catch (Exception e)
            {
                return new InternalAuthResponse { AuthoredPerson = null, Status = StatusAuthResponse.Error, Message = e.Message };
            }            
        }

        public async Task<bool> IsBlocked(string id)
        {
            var data = await DbSet.FindAsync(Builders<Person>.Filter.Eq("_id", id) & Builders<Person>.Filter.Eq("Excluded", false));
            return data.SingleOrDefault().Excluded;
        }

        public async Task<IEnumerable<Person>> New(DateTimeOffset date)
        {            
            var a = await DbSet.FindAsync(_ => _.Excluded == false && _.InsertDate >= date);

            return a.ToList();
        }

        public async Task<bool> IsAllowedToCreate(Person person)
        {
            var query = await DbSet.FindAsync(_ => _.Excluded == false && _.Username == person.Username);
            var personCursor = query.ToList();

            var tem = personCursor == null ? false : personCursor.Count == 0 ? false : personCursor.First() is Person;
            //var personCreated = await DbSet.FindAsync(_ => _.Username == person.Username);

            Log.Warning($"Is allowed to create? {!tem}");
            return !tem;
        }
    }
}