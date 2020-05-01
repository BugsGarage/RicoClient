using RicoClient.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Scripts.Network.Controllers
{
    public class CardsController
    {
        private readonly string _cardsServerUrl;

        public CardsController(CardsConfig configuration)
        {
            _cardsServerUrl = configuration.CardsServerURL;
        }

        public void GetAllCardsRequest()
        {

        }
    }
}
