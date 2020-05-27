using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

namespace RicoClient.Scripts.Game.InGameDeck
{
    public class InGameDeckScript : MonoBehaviour
    {
        [SerializeField]
        private int _yShift = 0;
        [SerializeField]
        private int _zShift = 0;

        [SerializeField]
        private GameObject _backCard = null;

        public int DeckCount { get { return _inDeckCards.Count; } }

        private List<GameObject> _inDeckCards;

        protected void OnDestroy()
        {
            if (_inDeckCards != null)
            {
                foreach (var inDeckCard in _inDeckCards)
                {
                    Destroy(inDeckCard);
                }
            }
        }

        public void SetDeck(int deckCount)
        {
            _inDeckCards = new List<GameObject>(deckCount);
            for (int i = 0; i < deckCount; i++)
            {
                var backcard = Instantiate(_backCard, transform);
                backcard.transform.localPosition = new Vector3(0, i * _yShift, i * _zShift);

                _inDeckCards.Add(backcard);
            }
        }

        public int TakeCard()
        {
            if (DeckCount > 0)
            {
                var topCard = _inDeckCards.Last();
                _inDeckCards.Remove(topCard);
                Destroy(topCard);
            }

            return DeckCount;
        }

        public int PutCard()
        {
            var backcard = Instantiate(_backCard, transform);
            backcard.transform.localPosition = new Vector3(0, _inDeckCards.Count * _yShift, _inDeckCards.Count * _zShift);
            _inDeckCards.Add(backcard);

            return DeckCount;
        }
    }
}
