using MongoDB.Bson.Serialization.Conventions;
using pressF.API.Repository.Mappings;

namespace pressF.API.Repository.Persistence
{
    public static class MongoDbPersistence
    {
        public static void Configure()
        {
            ProductMap.Configure();
            PersonMap.Configure();

            //BsonDefaults.GuidRepresentation = GuidRepresentation.CSharpLegacy;
            //BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));
            //BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;

            // Conventions
            var pack = new ConventionPack
                {
                    new IgnoreExtraElementsConvention(true),
                    new IgnoreIfDefaultConvention(true)
                };
            ConventionRegistry.Register("My Solution Conventions", pack, t => true);
        }
    }
}
