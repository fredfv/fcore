using MongoDB.Bson.Serialization;
using pressF.API.Model;

namespace pressF.API.Repository.Mappings
{
    public class ProductMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Product>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapMember(x => x.Description).SetIsRequired(true);
            });
        }
    }
}