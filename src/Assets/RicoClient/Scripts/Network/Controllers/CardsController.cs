using Newtonsoft.Json;
using RicoClient.Configs;
using RicoClient.Scripts.Cards;
using RicoClient.Scripts.Cards.Entities;
using RicoClient.Scripts.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx.Async;
using UnityEngine;
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

        public List<Card> GetAllCardsRequestMock()
        {
            using (var reader = new StreamReader("./Assets/RicoClient/Scripts/Network/RequestMocks/GetAllCards.json"))
            {
                string cardsJson = reader.ReadToEnd();

                return JsonConvert.DeserializeObject<List<Card>>(cardsJson);
            }
        }
    }
}
