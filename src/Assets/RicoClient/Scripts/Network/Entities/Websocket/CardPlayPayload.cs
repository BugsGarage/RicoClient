using Newtonsoft.Json;

namespace RicoClient.Scripts.Network.Entities.Websocket
{
    [JsonObject]
    public class CardPlayResponse
    {
        [JsonProperty("approved")]
        [JsonRequired]
        public bool Approved { get; set; }

        [JsonProperty("cid_id")]
        [JsonRequired]
        public int DeckCardId { get; set; }
    }
}
