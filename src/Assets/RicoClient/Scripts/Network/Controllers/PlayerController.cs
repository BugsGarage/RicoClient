using Newtonsoft.Json;
using RicoClient.Configs;
using RicoClient.Scripts.Decks;
using RicoClient.Scripts.Exceptions;
using RicoClient.Scripts.Network.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx.Async;
using UnityEngine.Networking;

namespace RicoClient.Scripts.Network.Controllers
{
    public class PlayerController
    {
        private readonly string _entranceEndpoint;
        private readonly string _playerInfoEndpoint;
        private readonly string _newDeckEndpoint;

        public PlayerController(PlayerConfig configuration)
        {
            _entranceEndpoint = configuration.EntranceEndpoint;
            _playerInfoEndpoint = configuration.PlayerInfoEndpoint;
            _newDeckEndpoint = configuration.NewDeckEndpoint;
        }

        public async UniTask PostEnterGame(string access_token)
        {
            using (var enterGameRequest = new UnityWebRequest(_entranceEndpoint, "POST"))
            {
                enterGameRequest.SetRequestHeader("Authorization", access_token);
                enterGameRequest.downloadHandler = new DownloadHandlerBuffer();

                await enterGameRequest.SendWebRequest();

                if (enterGameRequest.isNetworkError || enterGameRequest.isHttpError)
                    throw new PlayersException($"Error during entering in game: {enterGameRequest.error}. Restart app please!");
            }
        }

        public async UniTask<PlayerData> GetPlayerInfoRequest(string access_token)
        {
            using (var playerInfoRequest = new UnityWebRequest(_playerInfoEndpoint, "GET"))
            {
                playerInfoRequest.SetRequestHeader("Authorization", access_token);
                playerInfoRequest.downloadHandler = new DownloadHandlerBuffer();

                await playerInfoRequest.SendWebRequest();

                if (playerInfoRequest.isNetworkError || playerInfoRequest.isHttpError)
                    throw new PlayersException($"Error during getting player information: {playerInfoRequest.error}. Restart app please and try later!");

                return JsonConvert.DeserializeObject<PlayerData>(playerInfoRequest.downloadHandler.text);
            }
        }

        public async UniTask<Deck> GetDeckByIdRequest(string access_token, uint deckId)
        {
            string getDeckEndpoint = _newDeckEndpoint + "/" + deckId.ToString();

            using (var getDeckRequest = new UnityWebRequest(getDeckEndpoint, "GET"))
            {
                getDeckRequest.SetRequestHeader("Authorization", access_token);
                getDeckRequest.downloadHandler = new DownloadHandlerBuffer();

                await getDeckRequest.SendWebRequest();

                if (getDeckRequest.isNetworkError || getDeckRequest.isHttpError)
                    throw new PlayersException($"Error getting deck: {getDeckRequest.error}! Try later!");

                return JsonConvert.DeserializeObject<Deck>(getDeckRequest.downloadHandler.text);
            }
        }

        public async UniTask<uint> PostNewDeckRequest(string access_token, ConfirmDeck data)
        {
            string deckJson = JsonConvert.SerializeObject(data);

            using (var newDeckRequest = new UnityWebRequest(_newDeckEndpoint, "POST"))
            {
                newDeckRequest.SetRequestHeader("Authorization", access_token);
                newDeckRequest.SetRequestHeader("Content-Type", "application/json");
                byte[] bodyRaw = Encoding.UTF8.GetBytes(deckJson);
                newDeckRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                newDeckRequest.downloadHandler = new DownloadHandlerBuffer();

                await newDeckRequest.SendWebRequest();

                if (newDeckRequest.isNetworkError || newDeckRequest.isHttpError)
                    throw new PlayersException($"Error creating deck: {newDeckRequest.error}! Try later!");

                return uint.Parse(newDeckRequest.downloadHandler.text);
            }
        }

        public async UniTask PatchDeckByIdRequest(string access_token, uint deckId, ConfirmDeck data)
        {
            string editDeckEndpoint = _newDeckEndpoint + "/" + deckId.ToString();
            string deckJson = JsonConvert.SerializeObject(data);

            using (var editDeckRequest = new UnityWebRequest(editDeckEndpoint, "PATCH"))
            {
                editDeckRequest.SetRequestHeader("Authorization", access_token);
                editDeckRequest.SetRequestHeader("Content-Type", "application/json");
                byte[] bodyRaw = Encoding.UTF8.GetBytes(deckJson);
                editDeckRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                editDeckRequest.downloadHandler = new DownloadHandlerBuffer();

                await editDeckRequest.SendWebRequest();

                if (editDeckRequest.isNetworkError || editDeckRequest.isHttpError)
                    throw new PlayersException($"Error updating deck: {editDeckRequest.error}! Try later!");
            }
        }

        public async UniTask DeleteDeckByIdRequest(string access_token, uint deckId)
        {
            string deleteDeckEndpoint = _newDeckEndpoint + "/" + deckId.ToString();

            using (var deleteDeckRequest = new UnityWebRequest(deleteDeckEndpoint, "DELETE"))
            {
                deleteDeckRequest.SetRequestHeader("Authorization", access_token);
                deleteDeckRequest.downloadHandler = new DownloadHandlerBuffer();

                await deleteDeckRequest.SendWebRequest();

                if (deleteDeckRequest.isNetworkError || deleteDeckRequest.isHttpError)
                    throw new PlayersException($"Error deleting deck: {deleteDeckRequest.error}! Try later!");
            }
        }

        private PlayerData GetPlayerInfoRequestMock()
        {
            using (var reader = new StreamReader("./Assets/RicoClient/Scripts/Network/RequestMocks/GetPlayerInfo.json"))
            {
                string playerJson = reader.ReadToEnd();

                return JsonConvert.DeserializeObject<PlayerData>(playerJson);
            }
        }

        private Deck GetDeckByIdRequestMock()
        {
            using (var reader = new StreamReader("./Assets/RicoClient/Scripts/Network/RequestMocks/GetDeck.json"))
            {
                string deckJson = reader.ReadToEnd();

                return JsonConvert.DeserializeObject<Deck>(deckJson);
            }
        }
    }
}
