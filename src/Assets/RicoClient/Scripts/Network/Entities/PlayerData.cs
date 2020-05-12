using Newtonsoft.Json;
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
        public int Balance { get; set; }

        [JsonProperty("available_cards")]
        public Dictionary<int, int> OwnedCards { get; set; }

        [JsonProperty("decks")]
        public List<uint> Decks { get; set; }
    }
}
