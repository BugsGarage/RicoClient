using RicoClient.Scripts.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx.Async;

namespace RicoClient.Scripts.Decks
{
    public class DeckManager
    {
        private readonly NetworkManager _network;

        public Deck CurrDeck { get; private set; }

        public DeckManager(NetworkManager network)
        {
            _network = network;

            CurrDeck = null;
        }

        public async UniTask<Deck> DeckById(uint deckId)
        {
            CurrDeck = await _network.GetDeckById(deckId);

            return CurrDeck;
        }
    }
}
