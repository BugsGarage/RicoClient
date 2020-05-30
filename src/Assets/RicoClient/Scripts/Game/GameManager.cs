using NativeWebSocket;
using Newtonsoft.Json;
using RicoClient.Configs;
using RicoClient.Scripts.Exceptions;
using RicoClient.Scripts.Network;
using RicoClient.Scripts.Network.Entities;
using RicoClient.Scripts.User;
using System;
using System.Text;
using UniRx.Async;
using UnityEngine;

namespace RicoClient.Scripts.Game
{
    public class GameManager : IDisposable
    {
        public event Action OnWebsocketOpen;
        public event Action<WebSocketCloseCode> OnWebsocketClose;
        public event Action<WSResponse> OnWebsocketMessage;
        public event Action<string> OnWebsocketError;

        private readonly NetworkManager _network;

        private readonly string _gameWebsocketPath;

        /// <summary>
        /// Access token for in-game conversation with game service
        /// </summary>
        public string GameAccessToken { get; private set; }

        private WebSocket _gameWebsocket;

        public GameManager(NetworkManager network, GameConfig configuration)
        {
            _network = network;

            _gameWebsocketPath = configuration.GameWebsocketPath;
        }

        public async UniTask ConnectGame(uint deckId)
        {
            try
            {
                await _network.PostConnectGame(deckId);
            }
            catch (GameException)
            {
                throw;
            }

            GameAccessToken = UserManager.AccessToken;

            WebSocketSetup();
        }

        private async void WebSocketSetup()
        {
            if (_gameWebsocket != null && _gameWebsocket.State != WebSocketState.Closed)
                await _gameWebsocket.Close();

            _gameWebsocket = new WebSocket(_gameWebsocketPath);

            _gameWebsocket.OnOpen += () =>
            {
                Debug.Log("Connection open!");

                OnWebsocketOpen?.Invoke();
            };

            _gameWebsocket.OnError += (e) =>
            {
                Debug.Log("Error! " + e);

                OnWebsocketError?.Invoke(e);
            };

            _gameWebsocket.OnClose += (e) =>
            {
                Debug.Log("Connection closed! " + e);

                OnWebsocketClose?.Invoke(e);
            };

            _gameWebsocket.OnMessage += (bytes) =>
            {
                string message = Encoding.UTF8.GetString(bytes);
                Debug.Log("OnMessage! " + message);

                WSResponse resp = JsonConvert.DeserializeObject<WSResponse>(message);

                if (resp.Type != ResponseCommandType.Error)
                {
                    OnWebsocketMessage?.Invoke(resp);
                }
                else
                {
                    OnWebsocketError?.Invoke(resp.Error);
                }
            };

            // Waiting for messages
            await _gameWebsocket.Connect();
        }

        public async UniTask SendConnectionMessage()
        {
            await _gameWebsocket.SendText(JsonConvert.SerializeObject(new WSRequest(GameAccessToken, RequestCommandType.Connect, null)));
        }

        #region IDisposable Support

        private volatile bool _disposed = false;

        protected virtual async void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    await _gameWebsocket.Close();
                }
                _gameWebsocket = null;

                _disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
