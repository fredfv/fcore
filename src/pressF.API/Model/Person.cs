using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using pressF.API.Enums;
using pressF.API.Interfaces;
using pressF.API.ViewModel;
using System;

namespace pressF.API.Model
{
    public class Person : BaseDocument, ITraceable, IRemoveable
    {
        #region allowed
        public string Name { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        #endregion

        #region secured
        [BsonRepresentation(BsonType.Document)]
        public DateTimeOffset InsertDate { get; set; }
        [BsonRepresentation(BsonType.Document)]
        public DateTimeOffset? UpdateDate { get; set; }
        [BsonRepresentation(BsonType.Document)]
        public DateTimeOffset? ExcludedDate { get; set; }
        public bool Excluded { get; set; }
        #endregion

        public Person()
        {
        }

        public Person(PersonViewModel vm)
        {
            Name = vm.Name;
            Password = vm.Password;
            Username = vm.Username;
            Role = vm.Role;

            Excluded = false;
        }

        public Person(PersonViewModel vm, Person old)
        {
            //editable fields
            Name = vm.Name;
            Password = vm.Password;
            Username = vm.Username;
            Role = vm.Role;

            //base
            InsertDate = old.InsertDate;
            Id = old.Id;
            //removeable
            Excluded = old.Excluded;
            ExcludedDate = old.ExcludedDate;
        }
    }
}
