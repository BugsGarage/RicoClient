using Newtonsoft.Json;

namespace RicoClient.Scripts.Network.Entities.Websocket
{
    /// <summary>
    ///  Player card attack to service
    /// </summary>
    [JsonObject]
    public class AttackedPayload
    {
        [JsonProperty("cid_id")]
        [JsonRequired]
        public int DeckCardId { get; set; }

        [JsonProperty("target_cid_id")]
        public int TargetDeckCardId { get; set; }
    }
}
