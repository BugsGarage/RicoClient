using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Scripts.Network.Entities.Websocket
{
    [JsonObject]
    public class TakenCardPayload
    {
        [JsonProperty("card_id")]
        [JsonRequired]
        public int CardId { get; set; }

        [JsonProperty("card_deck_id")]
        [JsonRequired]
        public int CardDeckId { get; set; }
    }
}
