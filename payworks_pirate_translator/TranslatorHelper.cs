using Newtonsoft.Json;

/// <summary>
/// This classes model the incoming JSON. The incoming text response will be deserialized into this structure
/// </summary>
namespace payworks_pirate_translator
{
    public class ApiResponse
    {   
        [JsonProperty("translation")]
        public Translation Translation { get; set; }
    }

    public class Translation
    {
        [JsonProperty("english")]
        public string English { get; set; }

        [JsonProperty("pirate")]
        public string Pirate { get; set; }
    }
}
