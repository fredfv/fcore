using MongoDB.Bson.Serialization;
using pressF.API.Model;

namespace pressF.API.Repository.Mappings
{
    public class PersonMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Person>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
            });
        }
    }
}