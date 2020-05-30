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
using RicoClient.Scripts.Network.Entities;

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
            DisablePlayButton();

            uint currDeckId = UserManager.DeckHeaders[_decks.value].DeckId;
            try
            {
                await _game.ConnectGame(currDeckId);
            }
            catch (GameException e)
            {
                EnablePlayButton();

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

        private async void OnWebsocketConnected()
        {
            try
            {
                _playText.text = "Looking for opponents ...";
                await _game.SendConnectionMessage();
            }
            catch (Exception e)  // ToDo: Which exception
            {
                EnablePlayButton();

                Debug.LogError(e.Message);
                return;
            }
        }

        private void OnWebsocketError(string error)
        {
            EnablePlayButton();

            Debug.LogError($"Error during connecting game: {error}! Try later!");
        }

        private void OnWebsocketMessage(WSResponse msg)
        {
            switch (msg.Type)
            {
                case ResponseCommandType.Started:
                    SceneManager.LoadSceneAsync("RicoClient/Scenes/GameScene");
                    break;
            }
        }

        private void DisablePlayButton()
        {
            _playText.text = "Waiting..";
            _playButton.interactable = false;
        }

        private void EnablePlayButton()
        {
            _playText.text = "Play!";
            _playButton.interactable = true;
        }
    }
}
