using Newtonsoft.Json;

namespace Tests.DTOs
{
    class Secrets
    { 
        [JsonProperty("alchemyApiKey")]
        public string AlchemyApiKey { get; set; }
    }
}
