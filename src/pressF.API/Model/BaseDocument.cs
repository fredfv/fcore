using Newtonsoft.Json;

namespace pressF.API.Model
{
    public class BaseDocument
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }
    }
}