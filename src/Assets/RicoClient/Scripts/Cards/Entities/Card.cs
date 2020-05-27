using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Scripts.Cards.Entities
{
    [JsonObject]
    public class Card
    {
        /// <summary>
        /// Card ID
        /// </summary>
        [JsonProperty("card_id")]
        [JsonRequired]
        public int CardId { get; set; }

        /// <summary>
        /// Card name
        /// </summary>
        [JsonRequired]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Card rarity
        /// </summary>
        [JsonRequired]
        [JsonProperty("rarity")]
        public RarityType Rarity { get; set; }

        /// <summary>
        /// Card type (Unit, Building or Spell)
        /// </summary>
        [JsonRequired]
        [JsonProperty("type")]
        public CardType Type { get; set; }

        /// <summary>
        /// Card cost to be played
        /// </summary>
        [JsonProperty("cost")]
        public int Cost { get; set; }

        /// <summary>
        /// Card price in gold
        /// </summary>
        [JsonRequired]
        [JsonProperty("gold_cost")]
        public int GoldCost { get; set; }

        /// <summary>
        /// Card's properties (health, attack, etc.)
        /// </summary>
        [JsonRequired]
        [JsonProperty("properties")]
        public CardProperty Properties { get; set; }

        /// <summary>
        /// Card's ability (list in future?)
        /// </summary>
        [JsonProperty("ability")]
        public Ability Ability { get; set; }
    }
}
