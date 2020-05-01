using Newtonsoft.Json;
using RicoClient.Configs;
using RicoClient.Scripts.Exceptions;
using RicoClient.Scripts.User.Storage;
using RicoClient.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using UniRx.Async;
using UnityEngine;
using UnityEngine.Networking;

namespace RicoClient.Scripts.Network.Controllers
{
    public class AuthController
    {
        private const string code_challenge_method = "S256";
        
        private readonly string _authorizationEndpoint;
        private readonly string _tokenEndpoint;

        private readonly string _clientId;
        private readonly string _clientSecret;

        private readonly int _authorizationTimeoutSeconds;

        public AuthController(AuthorizationConfig configuration)
        {
            _authorizationEndpoint = configuration.AuthorizationEndpoint;
            _tokenEndpoint = configuration.TokenEndpoint;

            _clientId = configuration.ClientId;
            _clientSecret = configuration.ClientSecret;

            _authorizationTimeoutSeconds = configuration.AuthorizationTimeoutSeconds;
        }

        /// <summary>
        /// Making an OAuth2 authorization code flow request
        /// </summary>
        /// <returns>User tokens</returns>
        public async UniTask<TokenInfo> OAuthRequest()
        {
            // Generates state and PKCE values
            string state = RandomDataBase64url(32);
            string code_verifier = RandomDataBase64url(32);
            string code_challenge = Base64urlencodeNoPadding(Sha256(code_verifier));

            // Creates a redirect URI using an available port on the loopback address
            string redirectURI = $"http://{IPAddress.Loopback}:{GetRandomUnusedPort()}";
            Debug.Log("Redirect URI: " + redirectURI);

            string code = await GetAuthorizationCode(redirectURI, state, code_challenge);
            if (code == null)
                return new TokenInfo();

            // Starts the code exchange at the Token Endpoint
            return await PerformCodeExchange(code, code_verifier, redirectURI);
        }

        /// <summary>
        /// Making a refresh code request
        /// </summary>
        /// <param name="refresh_token"></param>
        /// <returns>User tokens</returns>
        public async UniTask<TokenInfo> RefreshTokenRequest(string refresh_token)
        {
            // Build request for token
            string tokenRequestBody = string.Format("refresh_token={0}&client_id={1}&client_secret={2}&scope=&grant_type=refresh_token",
                refresh_token,
                _clientId,
                _clientSecret
            );

            // Send the request
            using (UnityWebRequest tokenRequest = new UnityWebRequest(_tokenEndpoint, "POST"))
            {
                byte[] byteBody = Encoding.ASCII.GetBytes(tokenRequestBody);
                tokenRequest.uploadHandler = new UploadHandlerRaw(byteBody);
                tokenRequest.uploadHandler.contentType = "application/x-www-form-urlencoded";
                tokenRequest.downloadHandler = new DownloadHandlerBuffer();

                await tokenRequest.SendWebRequest();

                if (tokenRequest.isNetworkError || tokenRequest.isHttpError)
                    throw new OAuthException($"Error during getting refreshed tokens: {tokenRequest.error}. Restart app please!");
                else
                {
                    Debug.Log("Recieved refreshed tokens");

                    Dictionary<string, string> tokenEndpointDecoded = JsonConvert.DeserializeObject<Dictionary<string, string>>(tokenRequest.downloadHandler.text);

                    return new TokenInfo()
                    {
                        TokenType = tokenEndpointDecoded["token_type"],
                        AccessToken = tokenEndpointDecoded["access_token"],
                        ExpiresIn = int.Parse(tokenEndpointDecoded["expires_in"]),
                        RefreshToken = tokenEndpointDecoded["refresh_token"]
                    };
                }
            }
        }

        /// <summary>
        /// Gets authorization code from server
        /// </summary>
        /// <param name="redirectURI"></param>
        /// <param name="state"></param>
        /// <param name="code_challenge"></param>
        /// <returns>Authorization code</returns>
        private async UniTask<string> GetAuthorizationCode(string redirectURI, string state, string code_challenge)
        {
            // Creates the OAuth 2.0 authorization request
            string authorizationRequest = string.Format("{0}?response_type=code&scope=openid%20profile%20api1%20offline_access&" +
                "redirect_uri={1}&client_id={2}&state={3}&code_challenge={4}&code_challenge_method={5}",
                _authorizationEndpoint,
                Uri.EscapeDataString(redirectURI),
                _clientId,
                state,
                code_challenge,
                code_challenge_method);

            WindowsHelper.GetUnityId();

            var queryString = await GetOAuthCodeResponse(authorizationRequest, redirectURI);

            WindowsHelper.BringAppToFront();

            if (queryString == null)
                throw new OAuthException("Error during recieving authorization code! Try later..");
            if (queryString.Count == 0)
                return null;

            // Checks for errors
            string errors = queryString.Get("error");
            if (errors != null)
                throw new OAuthException($"Authorization error: {errors}. Restart app please!");

            // Extracts the code
            string code = queryString.Get("code");
            string incoming_state = queryString.Get("state");
            if (code == null || incoming_state == null)
                throw new OAuthException($"Malformed authorization response: {queryString}. Restart app please!");

            // Compares the receieved state to the expected value, to ensure that
            // this app made the request which resulted in authorization
            if (incoming_state != state)
                throw new OAuthException($"Received request with invalid state ({incoming_state}). Restart app please!");

            return code;
        }

        /// <summary>
        /// Get OAuth code response from the browser
        /// </summary>
        /// <param name="request"></param>
        /// <param name="redirectURI"></param>
        /// <returns>Code response</returns>
        private async UniTask<NameValueCollection> GetOAuthCodeResponse(string request, string redirectURI)
        {
            // Creates an HttpListener to listen for requests on that redirect URI
            var http = new HttpListener();
            http.Prefixes.Add($"{redirectURI}/");  // Dirty hack 'cause Prefix need / at the end and redirect_uri in the request don't
            http.Start();
            Debug.Log("HTTP server start listening..");

            Application.OpenURL(request);

            // Waits for the OAuth authorization response
            try
            {
                var context = await UniTask.Run(() =>
                {
                    return http.GetContext();
                }).Timeout(TimeSpan.FromSeconds(_authorizationTimeoutSeconds));
                SendResponseToBrowser(context.Response);

                return context.Request.QueryString;
            }
            catch (TimeoutException)
            {
                Debug.Log("Timeout of getting OAuth authorization code");
                return new NameValueCollection();
            }
            finally
            {
                http.Stop();
                Debug.Log("HTTP server stopped");
            }
        }

        /// <summary>
        /// Sending info to the browser for better UX
        /// </summary>
        /// <param name="response"></param>
        private async void SendResponseToBrowser(HttpListenerResponse response)
        {
            string responseString = "<html><head></head><body><h1>You are now being returned to the application.</h1><p>You may close this tab.</p></body></html>";
            var buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;

            using (var responseOutput = response.OutputStream)
            {
                await responseOutput.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        /// <summary>
        /// Perform authorization code exchange for tokens
        /// </summary>
        /// <param name="code"></param>
        /// <param name="code_verifier"></param>
        /// <param name="redirectURI"></param>
        /// <returns>Tokens</returns>
        private async UniTask<TokenInfo> PerformCodeExchange(string code, string code_verifier, string redirectURI)
        {
            Debug.Log("Exchanging code for tokens..");

            // Build request for token
            string tokenRequestBody = string.Format("code={0}&redirect_uri={1}&client_id={2}&code_verifier={3}&client_secret={4}&scope=&grant_type=authorization_code",
                code,
                Uri.EscapeDataString(redirectURI),
                _clientId,
                code_verifier,
                _clientSecret
            );

            // Send the request
            using (UnityWebRequest tokenRequest = new UnityWebRequest(_tokenEndpoint, "POST"))
            {
                byte[] byteBody = Encoding.ASCII.GetBytes(tokenRequestBody);
                tokenRequest.uploadHandler = new UploadHandlerRaw(byteBody);
                tokenRequest.uploadHandler.contentType = "application/x-www-form-urlencoded";
                tokenRequest.downloadHandler = new DownloadHandlerBuffer();

                await tokenRequest.SendWebRequest();

                if (tokenRequest.isNetworkError || tokenRequest.isHttpError)
                    throw new OAuthException($"Error during authorization: {tokenRequest.error}. Restart app please!");
                else
                {
                    Debug.Log("Recieved tokens");

                    Dictionary<string, string> tokenEndpointDecoded = JsonConvert.DeserializeObject<Dictionary<string, string>>(tokenRequest.downloadHandler.text);

                    return new TokenInfo()
                    {
                        TokenType = tokenEndpointDecoded["token_type"],
                        AccessToken = tokenEndpointDecoded["access_token"],
                        ExpiresIn = int.Parse(tokenEndpointDecoded["expires_in"]),
                        RefreshToken = tokenEndpointDecoded["refresh_token"]
                    };
                }
            }
        }

        /// <summary>
        /// Returns URI-safe data with a given input length.
        /// </summary>
        /// <param name="length">Input length (nb. output will be longer)</param>
        /// <returns></returns>
        private string RandomDataBase64url(uint length)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[length];
            rng.GetBytes(bytes);

            return Base64urlencodeNoPadding(bytes);
        }

        /// <summary>
        /// Base64url no-padding encodes the given input buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private string Base64urlencodeNoPadding(byte[] buffer)
        {
            string base64 = Convert.ToBase64String(buffer);

            // Converts base64 to base64url
            base64 = base64.Replace("+", "-");
            base64 = base64.Replace("/", "_");
            // Strips padding
            base64 = base64.Replace("=", "");

            return base64;
        }

        /// <summary>
        /// Returns the SHA256 hash of the input string.
        /// </summary>
        /// <param name="inputStirng"></param>
        /// <returns></returns>
        private byte[] Sha256(string inputStirng)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(inputStirng);
            SHA256Managed sha256 = new SHA256Managed();

            return sha256.ComputeHash(bytes);
        }

        /// <summary>
        /// Returns random unused port
        /// </summary>
        /// <returns></returns>
        private int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();

            return port;
        }
    }
}
