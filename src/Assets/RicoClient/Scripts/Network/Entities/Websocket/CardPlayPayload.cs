using Newtonsoft.Json;

namespace RicoClient.Scripts.Network.Entities.Websocket
{
    /// <summary>
    ///  Player card play from service
    /// </summary>
    [JsonObject]
    public class CardPlayPayload
    {
        [JsonProperty("approved")]
        [JsonRequired]
        public bool Approved { get; set; }

        [JsonProperty("cid_id")]
        [JsonRequired]
        public int DeckCardId { get; set; }
    }
}
