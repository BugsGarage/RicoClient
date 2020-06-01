using Newtonsoft.Json;

namespace RicoClient.Scripts.Network.Entities.Websocket
{
    /// <summary>
    /// Player card attack from service
    /// </summary>
    [JsonObject]
    public class AttackPayload
    {
        [JsonProperty("approved")]
        [JsonRequired]
        public bool Approved { get; set; }

        [JsonProperty("cid_id")]
        [JsonRequired]
        public int DeckCardId { get; set; }
    }
}
