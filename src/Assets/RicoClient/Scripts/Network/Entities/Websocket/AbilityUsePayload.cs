using Newtonsoft.Json;

namespace RicoClient.Scripts.Network.Entities.Websocket
{
    /// <summary>
    /// Player ability use from service
    /// </summary>
    [JsonObject]
    public class AbilityUsePayload
    {
        [JsonProperty("approved")]
        [JsonRequired]
        public bool Approved { get; set; }

        [JsonProperty("cid_id")]
        [JsonRequired]
        public int DeckCardId { get; set; }
    }
}
