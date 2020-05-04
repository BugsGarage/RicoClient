using Newtonsoft.Json;
using RicoClient.Scripts.Cards.Converters;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Scripts.Cards
{
    [JsonObject]
    public class Card
    {
        /// <summary>
        /// Card ID
        /// </summary>
        [JsonProperty("card_id")]
        [PrimaryKey]
        public uint CardId { get; set; }

        /// <summary>
        /// Card name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Card rarity
        /// </summary>
        [JsonProperty("rarity")]
        public string Rarity { get; set; }

        /// <summary>
        /// Card type (Unit, Building or Spell)
        /// </summary>
        [JsonProperty("type")]
        public CardType Type { get; set; }

        /// <summary>
        /// Card health if it should have it
        /// </summary>
        [JsonProperty("health")]
        public uint Health { get; set; }

        /// <summary>
        /// Card attack if it should have it
        /// </summary>
        [JsonProperty("attack")]
        public uint Attack { get; set; }

        /// <summary>
        /// Card initiative if it should have it
        /// </summary>
        [JsonProperty("initiative")]
        public uint Initiative { get; set; }

        /// <summary>
        /// Card resource generation if it should have it
        /// </summary>
        [JsonProperty("resource")]
        public uint Resource { get; set; }

        /// <summary>
        /// Card cost to be played
        /// </summary>
        [JsonProperty("cost")]
        public uint Cost { get; set; }

        /// <summary>
        /// Card ability type (what it is supposed to do)
        /// </summary>
        [JsonProperty("ability")]
        public AbilityType Ability { get; set; }

        /// <summary>
        /// Card ability activation type (when ability activates)
        /// </summary>
        [JsonProperty("ability_activation")]
        public AbilityActivationType AbilityActivation { get; set; }

        /// <summary>
        /// How ablity affects the target attack
        /// </summary>
        [JsonProperty("ability_attack")]
        public uint AbilityAttack { get; set; }

        /// <summary>
        /// How ability affects the target health
        /// </summary>
        [JsonProperty("ability_health")]
        public uint AbilityHealth { get; set; }

        /// <summary>
        /// Who could be targeted by ability (Friendly, Enemy or both)
        /// </summary>
        [JsonProperty("ability_target")]
        [JsonConverter(typeof(AbilityTargetTypeConverter))]
        public AbilityTargetType AbilityTarget { get; set; }

        /// <summary>
        /// Which kind of target could be targeted (CardType and its combination)
        /// </summary>
        [JsonProperty("ability_target_card_type")]
        [JsonConverter(typeof(CardTypeConverter))]
        public CardType AbilityTargetCardType { get; set; }

        /// <summary>
        /// How target is choosing (Pick, Random, specific card ids and etc)
        /// </summary>
        [JsonProperty("ability_targetness")]
        public AbilityTargetnessType AbilityTargetness { get; set; }

        /// <summary>
        /// How many targets to choose or how many targets affects (-1 for every of this ability type)
        /// </summary>
        [JsonProperty("ability_target_count")]
        public int AbilityTargetCount { get; set; }

        /// <summary>
        /// Card price in gold
        /// </summary>
        [JsonProperty("gold_cost")]
        public uint GoldCost { get; set; }
    }
}
