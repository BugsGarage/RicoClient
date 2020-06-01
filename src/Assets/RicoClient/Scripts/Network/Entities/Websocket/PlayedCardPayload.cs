using Newtonsoft.Json;

namespace RicoClient.Scripts.Network.Entities.Websocket
{
    [JsonObject]
    public class PlayedCardPayload
    {
        [JsonProperty("cid_id")]
        [JsonRequired]
        public int DeckCardId { get; set; }
    }
}
