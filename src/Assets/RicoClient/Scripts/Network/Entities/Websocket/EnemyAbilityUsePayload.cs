using Newtonsoft.Json;
using RicoClient.Scripts.Cards.Entities;

namespace RicoClient.Scripts.Network.Entities.Websocket
{
    [JsonObject]
    public class EnemyAbilityUsePayload
    {
        [JsonProperty("card_id")]
        [JsonRequired]
        public int CardId { get; set; }

        [JsonProperty("cid_id")]
        [JsonRequired]
        public int DeckCardId { get; set; }

        [JsonProperty("target_cid_id")]
        [JsonRequired]
        public int TargetDeckCardId { get; set; }

        [JsonProperty("target")]
        [JsonRequired]
        public AbilityTargetType Target { get; set; }
    }
}
