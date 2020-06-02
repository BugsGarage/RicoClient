using Newtonsoft.Json;

namespace RicoClient.Scripts.Network.Entities.Websocket
{
    /// <summary>
    ///  Player card play to service
    /// </summary>
    [JsonObject]
    public class PlayedCardPayload
    {
        [JsonProperty("cid_id")]
        [JsonRequired]
        public int DeckCardId { get; set; }
    }
}
