using Newtonsoft.Json;
using RicoClient.Configs;
using RicoClient.Scripts.Cards;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx.Async;
using UnityEngine;

namespace RicoClient.Scripts.Network.Controllers
{
    public class CardsController
    {
        private readonly string _cardsServerUrl;

        public CardsController(CardsConfig configuration)
        {
            _cardsServerUrl = configuration.CardsServerURL;
        }

        public async UniTask<List<Card>> GetAllCardsRequest(string accessToken)
        {
            // ToDo: Real request code
            return GetAllCardsRequestMock();
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
