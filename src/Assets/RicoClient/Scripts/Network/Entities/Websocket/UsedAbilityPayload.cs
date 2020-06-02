using Newtonsoft.Json;
using RicoClient.Scripts.Cards.Entities;

namespace RicoClient.Scripts.Network.Entities.Websocket
{
    /// <summary>
    ///  Player ability use to service
    /// </summary>
    [JsonObject]
    public class UsedAbilityPayload
    {
        [JsonProperty("cid_id")]
        public int DeckCardId { get; set; }

        [JsonProperty("target_cid_id")]
        public int TargetDeckCardId { get; set; }

        [JsonProperty("target")]
        public AbilityTargetType Target { get; set; }
    }
}
