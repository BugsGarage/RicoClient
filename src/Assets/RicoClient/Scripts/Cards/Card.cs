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
        [JsonRequired]
        [PrimaryKey]
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
        public string Rarity { get; set; }

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
        public uint Cost { get; set; }

        /// <summary>
        /// Card price in gold
        /// </summary>
        [JsonRequired]
        [JsonProperty("gold_cost")]
        public uint GoldCost { get; set; }

        /// <summary>
        /// Card's properties (health, attack, etc.)
        /// </summary>
        [JsonRequired]
        [JsonProperty("properties")]
        public CardProperty Properties { get; set; }

        /// <summary>
        /// Card's ability (list in future?)
        /// </summary>
        [JsonRequired]
        [JsonProperty("ability")]
        public Ability Ability { get; set; }
    }
}
