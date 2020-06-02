using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Scripts.Network.Entities.Websocket
{
    [JsonObject]
    public class EnemyStartTurnPayload
    {
        [JsonProperty("enemy_taken_cards_count")]
        [JsonRequired]
        public int EnemyTakenCards { get; set; }
    }
}
