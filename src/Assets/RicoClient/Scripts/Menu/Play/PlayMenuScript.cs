using RicoClient.Scripts.Game;
using RicoClient.Scripts.Exceptions;
using RicoClient.Scripts.Menu.Collection;
using RicoClient.Scripts.User;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using System;
using RicoClient.Scripts.Network.Entities.Websocket;
using Newtonsoft.Json;

namespace RicoClient.Scripts.Menu.Play
{
    public class PlayMenuScript : BaseMenuScript
    {
        private GameManager _game;

        [SerializeField]
        private CollectionMenuScript _collectionMenu = null;
        [SerializeField]
        private Button _playButton = null;
        [SerializeField]
        private Button _collectionButton = null;
        [SerializeField]
        private TMP_Text _playText = null;
        [SerializeField]
        private TMP_Dropdown _decks = null;

        [Inject]
        public void Initialize(GameManager game)
        {
            _game = game;
        }

        protected void OnEnable()
        {
            _playText.text = "Play!";

            List<string> deckNames = new List<string>(UserManager.DeckHeaders.Count);
            for (int i = 0; i < UserManager.DeckHeaders.Count; i++)
                deckNames.Add(UserManager.DeckHeaders[i].DeckName);
            _decks.AddOptions(deckNames);

            _decks.interactable = true;
            _collectionButton.interactable = true;

            if (_decks.options.Count > 0)
                _playButton.interactable = true;
            else
                _playButton.interactable = false;

            _game.OnWebsocketOpen += OnWebsocketConnected;
            _game.OnWebsocketError += OnWebsocketError;
            _game.OnWebsocketMessage += OnWebsocketMessage;
        }

        protected void OnDisable()
        {
            _game.OnWebsocketOpen -= OnWebsocketConnected;
            _game.OnWebsocketError -= OnWebsocketError;
            _game.OnWebsocketMessage -= OnWebsocketMessage;

            _decks.ClearOptions();
        }

        public async void OnPlayClick()
        {
            DisableButtons();

            uint currDeckId = UserManager.DeckHeaders[_decks.value].DeckId;
            try
            {
                await _game.ConnectGame(currDeckId);
            }
            catch (GameException e)
            {
                EnableButtons();

                Debug.LogError(e.Message);
                return;
            }
        }

        public async void OnCollectionClick()
        {
            try
            {
                await _user.UpdatePlayerInfo();
            }
            catch (PlayersException e)
            {
                Debug.LogError(e.Message);
                return;
            }

            _collectionMenu.ReturnMenu = this;
            _collectionMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        public async void OnBackClick()
        {
            await _game.CloseSocket();

            gameObject.SetActive(false);
        }

        private async void OnWebsocketConnected()
        {
            _playText.text = "Looking for opponents ...";

            try
            {
                await _game.SendConnectionMessage();
            }
            catch (Exception e)  // ToDo: is it needed
            {
                EnableButtons();

                Debug.LogError(e.Message);
                return;
            }
        }

        private void OnWebsocketError(string error)
        {
            EnableButtons();

            Debug.LogError($"Error during connecting game: {error}! Try later!");
        }

        private void OnWebsocketMessage(WSResponse msg)
        {
            switch (msg.Type)
            {
                case ResponseCommandType.Started:
                    _game.StartValues = JsonConvert.DeserializeObject<GameStartPayload>(msg.Payload.ToString());
                    _game.StartValues.MyNickname = UserManager.Username;
                    _game.StartValues.PlayerDeckInitSize = UserManager.DeckHeaders[_decks.value].CardsCount;

                    SceneManager.LoadSceneAsync("RicoClient/Scenes/GameScene");
                    break;
            }
        }

        private void DisableButtons()
        {
            _playText.text = "Waiting..";
            _playButton.interactable = false;
            _decks.interactable = false;
            _collectionButton.interactable = false;
        }

        private void EnableButtons()
        {
            _playText.text = "Play!";
            _playButton.interactable = true;
            _decks.interactable = true;
            _collectionButton.interactable = true;
        }
    }
}
