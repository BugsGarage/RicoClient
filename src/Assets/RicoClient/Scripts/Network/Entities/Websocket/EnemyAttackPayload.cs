using Newtonsoft.Json;

namespace RicoClient.Scripts.Network.Entities.Websocket
{
    /// <summary>
    /// Enemy card attack from service
    /// </summary>
    [JsonObject]
    public class EnemyAttackPayload
    {
        [JsonProperty("cid_id")]
        [JsonRequired]
        public int DeckCardId { get; set; }

        [JsonProperty("target_cid_id")]
        [JsonRequired]
        public int TargetDeckCardId { get; set; }
    }
}
