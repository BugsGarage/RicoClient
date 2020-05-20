using Newtonsoft.Json;
using RicoClient.Configs;
using RicoClient.Scripts.Decks;
using RicoClient.Scripts.Network.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx.Async;

namespace RicoClient.Scripts.Network.Controllers
{
    public class PlayerController
    {
        private readonly string _playerServerUrl;

        public PlayerController(PlayerConfig configuration)
        {
            _playerServerUrl = configuration.PlayerServerURL;
        }

        public async UniTask<PlayerData> GetPlayerInfoRequest(string accessToken)
        {
            // ToDo: Real request code
            return GetPlayerInfoRequestMock();
        }

        public async UniTask<Deck> GetDeckByIdRequest(string accessToken, uint deckId)
        {
            // ToDo: Real request code
            return GetDeckByIdRequestMock();
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
