using Newtonsoft.Json;

namespace RicoClient.Scripts.Network.Entities.Websocket
{
    [JsonObject]
    public class WSResponse
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("type")]
        [JsonRequired]
        public ResponseCommandType Type { get; set; }

        [JsonProperty("payload")]
        public object Payload { get; set; }
    }
}
