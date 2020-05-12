using RicoClient.Configs;
using RicoClient.Scripts.Cards.Entities;
using RicoClient.Scripts.Network;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UniRx.Async;
using UnityEngine;

namespace RicoClient.Scripts.Cards
{
    public class CardsManager 
    {
        private readonly NetworkManager _network;

        public List<Card> AllCards { get; private set; } 

        public CardsManager(NetworkManager network)
        {
            _network = network;
            AllCards = new List<Card>();
        }

        /// <summary>
        /// Updating local cards database from the cards server
        /// </summary>
        public async UniTask UpdateLocalCards()
        {
            AllCards = await _network.GetAllCards();
        }

        public Card[] GetCardsRangeFrom(List<int> ids, int startId, int count)
        {
            Card[] cards = new Card[count];
            for (int i = 0, j = 1; i < count; j++)
            {
                if (j == ids[i + startId - 1])
                {
                    cards[i] = AllCards[j - 1];
                    i++;
                }
            }

            return cards;
        }

        public Card[] GetAllCardsRange(int startId, int count)
        {
            Card[] cards = new Card[count];
            for (int i = 0; i < count; i++)
            {
                cards[i] = AllCards[startId + i - 1];
            }

            return cards;
        }
    }
}
