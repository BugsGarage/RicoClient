using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Scripts.Cards.Entities
{
    [JsonObject]
    public class Ability
    {
        /// <summary>
        /// Card ability type (what it is supposed to do)
        /// </summary>
        [JsonRequired]
        [JsonProperty("type")]
        public AbilityType Type { get; set; }

        /// <summary>
        /// Card ability activation type (when ability activates)
        /// </summary>
        [JsonRequired]
        [JsonProperty("activation")]
        public AbilityActivationType Activation { get; set; }

        /// <summary>
        /// Who could be targeted by ability (Friendly, Enemy or both)
        /// </summary>
        [JsonRequired]
        [JsonProperty("target")]
        public AbilityTargetType Target { get; set; }

        /// <summary>
        /// Which kind of target could be targeted (CardType and its combination)
        /// </summary>
        [JsonRequired]
        [JsonProperty("target_type")]
        public CardType TargetType { get; set; }

        /// <summary>
        /// How target is choosing (Pick, Random, specific card ids and etc)
        /// </summary>
        [JsonRequired]
        [JsonProperty("targetness")]
        public AbilityTargetnessType Targetness { get; set; }

        /// <summary>
        /// How many targets to choose or how many targets affects (-1 for every of this ability target type)
        /// </summary>
        [JsonRequired]
        [JsonProperty("target_count")]
        public int TargetCount { get; set; }

        /// <summary>
        /// How ablity affects the target attack
        /// </summary>
        [JsonRequired]
        [JsonProperty("attack")]
        public int Attack { get; set; }

        /// <summary>
        /// How ability affects the target health
        /// </summary>
        [JsonRequired]
        [JsonProperty("health")]
        public int Health { get; set; }
    }
}
