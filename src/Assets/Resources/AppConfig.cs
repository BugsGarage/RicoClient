using System.Collections;
using System.Collections.Generic;
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
        [Tooltip("How long keep context for authorization code answer")]
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

        public string CardsServerURL { get { return _cardsServerURL; } }

        #endregion

        #region Player Configuration

        [Header("Player Configuration")]

        [SerializeField]
        private string _playerServerURL = null;

        public string PlayerServerURL { get { return _playerServerURL; } }

        #endregion

        #region Shop and Payment Configuration

        [Header("Shop and Payment Configuration")]

        [SerializeField]
        private string _shopServerURL = null;
        [SerializeField]
        private string _paymentServerURL = null;

        public string ShopServerURL { get { return _shopServerURL; } }
        public string PaymentServerURL { get { return _paymentServerURL; } }

        #endregion
    }
}