using RicoClient.Scripts.Exceptions;
using RicoClient.Scripts.Menu.Collection;
using RicoClient.Scripts.User;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RicoClient.Scripts.Menu.Play
{
    public class PlayMenuScript : BaseMenuScript
    {
        [SerializeField]
        private CollectionMenuScript _collectionMenu = null;
        [SerializeField]
        private Button _playButton = null;
        [SerializeField]
        private TMP_Text _playText = null;
        [SerializeField]
        private TMP_Dropdown _decks = null;

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
        }

        protected void OnDisable()
        {
            _decks.ClearOptions();
        }

        public async void OnPlayClick()
        {
            _playText.text = "Waiting..";
            _playButton.interactable = false;

            // Start match look up logic

            SceneManager.LoadSceneAsync("RicoClient/Scenes/GameScene");
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
    }
}
