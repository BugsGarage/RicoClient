using Newtonsoft.Json;

namespace RicoClient.Scripts.Network.Entities.Websocket
{
    [JsonObject]
    public class GameFinishPayload
    {
        [JsonProperty("isWinner")]
        [JsonRequired]
        public bool IsWinner { get; set; }
    }
}
