using Newtonsoft.Json;

namespace RicoClient.Scripts.Network.Entities
{
    [JsonObject]
    public class WSRequest
    {
        [JsonProperty("token")]
        public string Token { get; private set; }

        [JsonProperty("type")]
        public RequestCommandType Type { get; private set; }

        [JsonProperty("payload")]
        public object Payload { get; private set; }

        public WSRequest(string token, RequestCommandType type, object payload)
        {
            Token = token;
            Type = type;
            Payload = payload;
        }
    }
}
