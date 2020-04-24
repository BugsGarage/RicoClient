using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RicoClient
{
    [CreateAssetMenu]
    public class AppConfig : ScriptableObject
    {
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
    }
}