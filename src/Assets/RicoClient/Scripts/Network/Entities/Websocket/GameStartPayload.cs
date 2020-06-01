using Newtonsoft.Json;
using RicoClient.Scripts.Cards.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Scripts.Network.Entities.Websocket
{
    [JsonObject]
    public class GameStartPayload
    {
        /// <summary>
        /// My name
        /// </summary>
        public string MyNickname { get; set; }

        /// <summary>
        /// Enemy's name
        /// </summary>
        [JsonProperty("enemy_name")]
        [JsonRequired]
        public string EnemyNickname { get; set; }

        /// <summary>
        /// Initial size of players deck
        /// </summary>
        public int PlayerDeckInitSize { get; set; }

        /// <summary>
        /// Initial size of enemy deck
        /// </summary>
        [JsonProperty("enemy_deck_count")]
        [JsonRequired]
        public int EnemyDeckInitSize { get; set; }

        /// <summary>
        /// List of cards I took
        /// </summary>
        [JsonProperty("player_taken_cards")]
        [JsonRequired]
        public List<TakenCardPayload> MyStartCards { get; set; }

        /// <summary>
        /// How much enemy takes cards in game start
        /// </summary>
        [JsonProperty("enemy_taken_cards_count")]
        [JsonRequired]
        public int EnemyStartCardsCount { get; set; }

        /// <summary>
        /// Information about my base
        /// </summary>
        [JsonProperty("player_base")]
        [JsonRequired]
        public BaseBuilding MyBase { get; set; }

        /// <summary>
        /// Information about enemy base
        /// </summary>
        [JsonProperty("enemy_base")]
        [JsonRequired]
        public BaseBuilding EnemyBase { get; set; }

        /// <summary>
        /// Is I am taking the first turn
        /// </summary>
        [JsonProperty("IsFirst")]
        [JsonRequired]
        public bool IsFirst { get; set; }
    }
}
