using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Scripts.Cards.Entities
{
    [JsonObject]
    public class BaseBuilding
    {
        /// <summary>
        /// Base health
        /// </summary>
        [JsonRequired]
        [JsonProperty("health_points")]
        public int Health { get; set; }

        /// <summary>
        /// Base resource generation
        /// </summary>
        [JsonRequired]
        [JsonProperty("resourse_points")]
        public int Resources { get; set; }
    }
}
