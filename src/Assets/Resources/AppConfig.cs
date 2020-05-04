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
        [Tooltip("How long keep context for authorization code answer")]
        private int _authorizationTimeoutSeconds = 300;

        public string AuthServerURL { get { return _authServerURL; } }
        public string AuthorizationEndpoint { get { return $"{_authServerURL}{_authorizationEndpoint}"; } }
        public string TokenEndpoint { get { return $"{_authServerURL}{_tokenEndpoint}"; } }

        public string ClientId { get { return _clientId; } }
        public string ClientSecret { get { return _clientSecret; } }

        public int AuthorizationTimeoutSeconds { get { return _authorizationTimeoutSeconds; } }

        #endregion

        #region Cards Configuration

        [Header("Cards Configuration")]

        [SerializeField]
        private string _cardsServerURL = null;

        [SerializeField]
        [Tooltip("Must be in StreamingAssests folder")]
        private string _cardLocalDBPath = null;

        public string CardsServerURL { get { return _cardsServerURL; } }

        public string CardLocalDBPath { get { return _cardLocalDBPath; } }

        #endregion
    }
}