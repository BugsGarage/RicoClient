using Newtonsoft.Json;

namespace RicoClient.Scripts.Network.Entities.Websocket
{
    [JsonObject]
    public class EnemyCardPlayPayload
    {
        [JsonProperty("card_id")]
        [JsonRequired]
        public int CardId { get; set; }

        [JsonProperty("cid_id")]
        [JsonRequired]
        public int DeckCardId { get; set; }
    }
}
