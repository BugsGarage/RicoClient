using Newtonsoft.Json;
using RicoClient.Scripts.Decks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Scripts.Network.Entities
{
    [JsonObject]
    public class PlayerData
    {
        [JsonProperty("balance")]
        [JsonRequired]
        public int Balance { get; set; }

        [JsonProperty("available_cards")]
        [JsonRequired]
        public Dictionary<int, int> OwnedCards { get; set; }

        [JsonProperty("decks")]
        [JsonRequired]
        public List<DeckHeader> Decks { get; set; }
    }
}
