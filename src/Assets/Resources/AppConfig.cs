using UnityEngine;

namespace RicoClient
{
    [CreateAssetMenu]
    public class AppConfig : ScriptableObject
    {
        #region Authorization Configuration

        [Header("Authorization Configuration")]

        [SerializeField]
        private string _authServerURL = null;

        [SerializeField]
        private string _authorizationEndpoint = null;
        [SerializeField]
        private string _tokenEndpoint = null;

        [SerializeField]
        private string _clientId = null;
        [SerializeField]
        private string _clientSecret = null;

        [SerializeField]
        private string[] _scopes = null;

        [SerializeField]
        [Tooltip("How long keep context for authorization code answer (wait for response from browser)")]
        private int _authorizationTimeoutSeconds = 300;

        public string AuthServerURL { get { return _authServerURL; } }

        public string AuthorizationEndpoint { get { return $"{_authServerURL}{_authorizationEndpoint}"; } }
        public string TokenEndpoint { get { return $"{_authServerURL}{_tokenEndpoint}"; } }

        public string ClientId { get { return _clientId; } }
        public string ClientSecret { get { return _clientSecret; } }

        public string[] Scopes { get { return _scopes; } }

        public int AuthorizationTimeoutSeconds { get { return _authorizationTimeoutSeconds; } }

        #endregion

        #region Cards Configuration

        [Header("Cards Configuration")]

        [SerializeField]
        private string _cardsServerURL = null;

        [SerializeField]
        private string _allCardsEndpoint = null;

        public string CardsServerURL { get { return _cardsServerURL; } }

        public string AllCardsEndpoint { get { return $"{_cardsServerURL}{_allCardsEndpoint}"; } }

        #endregion

        #region Player Configuration

        [Header("Player Configuration")]

        [SerializeField]
        private string _playerServerURL = null;

        [SerializeField]
        private string _entranceEndpoint = null;
        [SerializeField]
        private string _playerInfoEndpoint = null;
        [SerializeField]
        private string _newDeckEndpoint = null;

        public string PlayerServerURL { get { return _playerServerURL; } }

        public string EntranceEndpoint { get { return $"{_playerServerURL}{_entranceEndpoint}"; } }
        public string PlayerInfoEndpoint { get { return $"{_playerServerURL}{_playerInfoEndpoint}"; } }
        public string NewDeckEndpoint { get { return $"{_playerServerURL}{_newDeckEndpoint}"; } }

        #endregion

        #region Shop Configuration

        [Header("Shop Configuration")]

        [SerializeField]
        private string _shopServerURL = null;

        [SerializeField]
        private string _specificCardEndpoint = null;
        [SerializeField]
        private string _randomCardEndpoint = null;
        [SerializeField]
        private string _randomCostEndpoint = null;

        public string ShopServerURL { get { return _shopServerURL; } }

        public string SpecificCardEndpoint { get { return $"{_shopServerURL}{_specificCardEndpoint}"; } }
        public string RandomCardEndpoint { get { return $"{_shopServerURL}{_randomCardEndpoint}"; } }
        public string RandomCostEndpoint { get { return $"{_shopServerURL}{_randomCostEndpoint}"; } }

        #endregion

        #region Payment Configuration

        [Header("Payment Configuration")]

        [SerializeField]
        private string _paymentServerURL = null;

        [SerializeField]
        private string _goldCoefEndpoint = null;
        [SerializeField]
        private string _paymentEndpoint = null;

        public string PaymentServerURL { get { return _paymentServerURL; } }

        public string GoldCoefEndpoint { get { return $"{_paymentServerURL}{_goldCoefEndpoint}"; } }
        public string PaymentEndpoint { get { return $"{_paymentServerURL}{_paymentEndpoint}"; } }

        #endregion

        #region Game Configuration

        [Header("Game Configuration")]

        [SerializeField]
        private string _gameServerURL = null;

        [SerializeField]
        private string _gameConnectEndpoint = null;

        [SerializeField]
        private string _gameWebsocketPath = null;

        public string GameServerURL { get { return _gameServerURL; } }

        public string GameConnectEndpoint { get { return $"{_gameServerURL}{_gameConnectEndpoint}"; } }

        public string GameWebsocketPath { get { return _gameWebsocketPath; } }

        #endregion
    }
}