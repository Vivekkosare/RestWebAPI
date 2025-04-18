﻿using Newtonsoft.Json;

namespace RestWebAPI.Entities
{
    public class Phone
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("data")]
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

    }


}
