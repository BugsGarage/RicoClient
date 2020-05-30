using Newtonsoft.Json;
using RicoClient.Configs;
using RicoClient.Scripts.Cards.Entities;
using RicoClient.Scripts.Exceptions;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine.Networking;

namespace RicoClient.Scripts.Network.Controllers
{
    public class CardsController
    {
        private readonly string _allCardsEndpoint;

        public CardsController(CardsConfig configuration)
        {
            _allCardsEndpoint = configuration.AllCardsEndpoint;
        }

        public async UniTask<List<Card>> GetAllCardsRequest() 
        {
            using (var allCardsRequest = new UnityWebRequest(_allCardsEndpoint, "GET"))
            {
                allCardsRequest.downloadHandler = new DownloadHandlerBuffer();

                await allCardsRequest.SendWebRequest();

                if (allCardsRequest.isNetworkError || allCardsRequest.isHttpError)
                    throw new CardsException($"Error during entering in game: {allCardsRequest.error}. Restart app please!");

                return JsonConvert.DeserializeObject<List<Card>>(allCardsRequest.downloadHandler.text);
            }
        }
    }
}
