using RicoClient.Scripts.Menu.Collection;
using RicoClient.Scripts.Network;
using RicoClient.Scripts.User;
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
        private readonly UserManager _user;

        public Deck CurrDeck { get; private set; }

        public DeckManager(NetworkManager network, UserManager user)
        {
            _network = network;
            _user = user;

            CurrDeck = null;
        }

        public async UniTask<Deck> DeckById(uint deckId)
        {
            CurrDeck = await _network.GetDeckById(deckId);

            return CurrDeck;
        }

        public async UniTask AddDeck(string deckName, Dictionary<int, int> deckCards, int cardsCount)
        {
            uint deckId = await _network.PostNewDeck(deckName, deckCards);
            _user.UpdateLocalDeck(deckId, deckName, cardsCount);

            CurrDeck = null;
        }

        public async UniTask ConfirmDeckChange(uint deckId, string deckName, Dictionary<int, int> deckCards, int cardsCount)
        {
            await _network.PatchDeckById(deckId, deckName, deckCards);
            _user.UpdateLocalDeck(deckId, deckName, cardsCount);

            CurrDeck = null;
        }

        public async UniTask DeleteDeck(uint deckId)
        {
            await _network.DeleteDeckById(deckId);
            _user.DeleteLocalDeck(deckId);

            CurrDeck = null;
        }
    }
}
