namespace pressF.API.Repository
{
    public class MongoSettings : IMongoSettings
    {
        public string Connection { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IMongoSettings
    {
        string Connection { get; set; }
        string DatabaseName { get; set; }
    }
}
