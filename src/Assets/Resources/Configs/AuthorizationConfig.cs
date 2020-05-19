using RicoClient;
using System;
using System.Collections.Generic; 
using System.Linq; 
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Configs
{
    public class AuthorizationConfig
    {
        public string AuthServerURL { get; }
        public string AuthorizationEndpoint { get; }
        public string TokenEndpoint { get; }

        public string ClientId { get; }
        public string ClientSecret { get; }

        public string[] Scopes { get; }

        public int AuthorizationTimeoutSeconds { get; }

        public AuthorizationConfig(AppConfig config)
        {
            AuthServerURL = config.AuthServerURL;
            AuthorizationEndpoint = config.AuthorizationEndpoint;
            TokenEndpoint = config.TokenEndpoint;

            ClientId = config.ClientId;
            ClientSecret = config.ClientSecret;

            Scopes = config.Scopes;

            AuthorizationTimeoutSeconds = config.AuthorizationTimeoutSeconds;
        }
    }
}
