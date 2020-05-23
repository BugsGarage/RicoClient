using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Scripts.Decks
{
    [JsonObject]
    public class Deck
    {
        [JsonProperty("deck_id")]
        [JsonRequired]
        public uint DeckId { get; set; }

        [JsonProperty("deck_name")]
        [JsonRequired]
        public string DeckName { get; set; }

        [JsonProperty("cards_count")]
        [JsonRequired]
        public int CardsCount { get; set; }

        [JsonProperty("deck_cards")]
        [JsonRequired]
        public Dictionary<int, int> DeckCards { get; set; }
    }
}
