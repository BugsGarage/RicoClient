using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Scripts.Network.Entities
{
    [JsonObject]
    public class ConfirmDeck
    {
        [JsonProperty("deck_name")]
        [JsonRequired]
        public string DeckName { get; set; }

        [JsonProperty("deck_cards")]
        [JsonRequired]
        public Dictionary<int, int> DeckCards { get; set; }
    }
}
