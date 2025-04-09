using Newtonsoft.Json;

namespace RestWebAPI.Entities
{
    public record PhoneInput
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("data")]
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();

    }
}
