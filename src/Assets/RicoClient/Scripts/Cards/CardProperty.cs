using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Scripts.Cards
{
    [JsonObject]
    public class CardProperty
    {
        /// <summary>
        /// Card attack if it should have it
        /// </summary>
        [JsonRequired]
        [JsonProperty("attack")]
        public int Attack { get; set; }

        /// <summary>
        /// Card health if it should have it
        /// </summary>
        [JsonRequired]
        [JsonProperty("health")]
        public int Health { get; set; }

        /// <summary>
        /// Card initiative if it should have it
        /// </summary>
        [JsonRequired]
        [JsonProperty("initiative")]
        public int Initiative { get; set; }

        /// <summary>
        /// Card resource generation if it should have it
        /// </summary>
        [JsonRequired]
        [JsonProperty("resources")]
        public int Resources { get; set; }
    }
}
