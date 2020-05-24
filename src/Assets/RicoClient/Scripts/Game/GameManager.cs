using RicoClient.Scripts.Cards;
using RicoClient.Scripts.Cards.Entities;
using RicoClient.Scripts.Decks;
using RicoClient.Scripts.Exceptions;
using RicoClient.Scripts.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx.Async;
using UnityEngine;

namespace RicoClient.Scripts.Game
{
    public enum GameState
    {
        Start,
        TurnStart,
        Preparing,
        Battle,
        TurnEnd,
        Finished
    }

    public class GameManager
    {
        private readonly NetworkManager _network;
        private readonly CardsManager _cards;
        private readonly DeckManager _deck;

        public GameState State { get; private set; }

        // Temp
        public Deck PlayerDeck { get; private set; }

        public int PlayerDeckInitSize { get; private set; }
        public int EnemyDeckInitSize { get; private set; }

        private int _collectedTurnStartCardsCount = 1;
        private int _gameStartCardsCount = 3;

        private bool _isGameStarted = false;

        public GameManager(NetworkManager network, CardsManager cards, DeckManager deck)
        {
            _network = network;
            _cards = cards;
            _deck = deck;

            State = GameState.Finished;
            
            // Also temp
            _cards.UpdateLocalCards();
        }

        public async UniTask StartGame()
        {
            if (State != GameState.Finished)
                throw new GameCoreException("Not finished previous game!");

            // Not sure how to start so let it be that way for now
            PlayerDeck = await _deck.DeckById(4);
            PlayerDeckInitSize = PlayerDeck.CardsCount;
            EnemyDeckInitSize = 10;
   
            State = GameState.Start;
        }

        public List<Card> TurnStart()
        {
            if (State == GameState.TurnEnd)
            {
                List<Card> collectedTurnStartCards = new List<Card>(_collectedTurnStartCardsCount);



                State = GameState.TurnStart;
                return collectedTurnStartCards;
            }
            
            if (State == GameState.Start)
            {
                List<Card> gameStartCards = new List<Card>(_gameStartCardsCount);

                for (int i = 0; i < _gameStartCardsCount; i++)
                {
                    // This is temp too
                    gameStartCards.Add(GetRandomCardFromDeckMock());
                }

                _isGameStarted = true;
                State = GameState.TurnStart;
                return gameStartCards;
            }

            throw new GameCoreException("Bad game state for turn start!");
        }

        public int TurnStartEnemy()
        {
            if (State == GameState.TurnStart)
            {
                if (_isGameStarted)
                    return _gameStartCardsCount;
                else
                    return _collectedTurnStartCardsCount;
            }

            throw new GameCoreException("Bad game state for enemy turn start!");
        }

        public void Preparing()
        {
            if (State != GameState.TurnStart)
                throw new GameCoreException("Not finished previous game!");

            State = GameState.Preparing;
        }

        // Temp mock
        private Card GetRandomCardFromDeckMock()
        {
            int randomCardI = UnityEngine.Random.Range(0, PlayerDeck.DeckCards.Count);
            return _cards.GetCardById(PlayerDeck.DeckCards.Keys.ElementAt(randomCardI));
        }
    }
}
