using RicoClient.Configs;
using RicoClient.Scripts.Cards.Entities;
using RicoClient.Scripts.Network;
using RicoClient.Scripts.Utils;
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
            AllCards.Sort(new CardsComparer());
        }

        public Card[] GetCardsRangeFrom(List<int> ids, int startId, int count)
        {
            Card[] cards = new Card[count];
            int lastId = startId + count;
            for (int i = startId; i < lastId; i++)
            {
                cards[i - startId] = GetCardById(ids[i]);
            }

            return cards;
        }

        public Card[] GetAllCardsRange(int startId, int count)
        {
            Card[] cards = new Card[count];
            for (int i = 0; i < count; i++)
            {
                cards[i] = AllCards[startId + i];
            }

            return cards;
        }

        public Card GetCardById(int id)
        {
            for (int i = 0; i < AllCards.Count; i++)
            {
                if (AllCards[i].CardId == id)
                    return AllCards[i];
            }

            return null;
        }
    }
}
