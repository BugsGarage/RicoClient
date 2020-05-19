using RicoClient.Scripts.Decks;
using System.Collections.Generic;

namespace RicoClient.Scripts.User.Storage
{
    public struct UserStorage 
    {
        public string Username { get; set; }

        public TokenInfo Tokens { get; set; }

        public int BalanceValue { get; set; }

        public Dictionary<int, int> OwnedCards { get; set; }

        public List<DeckHeader> Decks { get; set; }
    }
}
