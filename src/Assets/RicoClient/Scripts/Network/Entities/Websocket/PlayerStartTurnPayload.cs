using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Scripts.Network.Entities.Websocket
{
    [JsonObject]
    public class PlayerStartTurnPayload
    {
        [JsonProperty("player_taken_cards")]
        [JsonRequired]
        public List<TakenCardPayload> PlayerTakenCards { get; set; }
    }
}
