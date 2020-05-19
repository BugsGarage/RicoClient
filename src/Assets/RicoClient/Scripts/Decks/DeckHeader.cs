using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Scripts.Decks
{
    [JsonObject]
    public class DeckHeader
    {
        [JsonProperty("deck_id")]
        public uint DeckId { get; set; }

        [JsonProperty("deck_name")]
        public string DeckName { get; set; }

        [JsonProperty("cards_count")]
        public int CardsCount { get; set; }
    }
}
