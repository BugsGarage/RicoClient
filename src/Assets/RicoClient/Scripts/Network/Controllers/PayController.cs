using Newtonsoft.Json;
using RicoClient.Configs;
using RicoClient.Scripts.Exceptions;
using System.Globalization;
using System.Text;
using UniRx.Async;
using UnityEngine.Networking;

namespace RicoClient.Scripts.Network.Controllers
{
    public class PayController
    {
        private readonly string _specificCardEndpoint;
        private readonly string _randomCardEndpoint;
        private readonly string _randomCostEndpoint;

        private readonly string _goldCoefEndpoint;
        private readonly string _paymentEndpoint;

        public PayController(ShopConfig shopConfiguration, PayConfig paymentConfiguration)
        {
            _specificCardEndpoint = shopConfiguration.SpecificCardEndpoint;
            _randomCardEndpoint = shopConfiguration.RandomCardEndpoint;
            _randomCostEndpoint = shopConfiguration.RandomCostEndpoint;

            _goldCoefEndpoint = paymentConfiguration.GoldCoefEndpoint;
            _paymentEndpoint = paymentConfiguration.PaymentEndpoint;
        }

        public async UniTask<int> GetRandomCardCostRequest()
        {
            using (var randomCostRequest = new UnityWebRequest(_randomCostEndpoint, "GET"))
            {
                randomCostRequest.downloadHandler = new DownloadHandlerBuffer();

                await randomCostRequest.SendWebRequest();

                if (randomCostRequest.isNetworkError || randomCostRequest.isHttpError)
                    throw new ShopException($"Error connecting shop service: {randomCostRequest.error}! Try later!");

                return int.Parse(randomCostRequest.downloadHandler.text);
            }
        }

        public async UniTask<double> GetGoldCoefRequest()
        {
            using (var goldCoefRequest = new UnityWebRequest(_goldCoefEndpoint, "GET"))
            {
                goldCoefRequest.downloadHandler = new DownloadHandlerBuffer();

                await goldCoefRequest.SendWebRequest();

                if (goldCoefRequest.isNetworkError || goldCoefRequest.isHttpError)
                    throw new PaymentException($"Error connecting payment service: {goldCoefRequest.error}! Try later!");

                return double.Parse(goldCoefRequest.downloadHandler.text, CultureInfo.InvariantCulture);
            }
        }

        public async UniTask PostBuySpecificCardRequest(string access_token, int cardId)
        {
            string jsonBody = JsonConvert.SerializeObject(new { card_id = cardId });

            using (var specificCardRequest = new UnityWebRequest(_specificCardEndpoint, "POST"))
            {
                specificCardRequest.SetRequestHeader("Authorization", access_token);
                specificCardRequest.SetRequestHeader("Content-Type", "application/json");
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
                specificCardRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                specificCardRequest.downloadHandler = new DownloadHandlerBuffer();

                await specificCardRequest.SendWebRequest();

                if (specificCardRequest.isNetworkError || specificCardRequest.isHttpError)
                    throw new ShopException($"Error buying card: {specificCardRequest.error}! Try later!");
            }
        }

        public async UniTask<int> PostBuyRandomCardRequest(string access_token)
        {
            using (var randomCardRequest = new UnityWebRequest(_randomCardEndpoint, "POST"))
            {
                randomCardRequest.SetRequestHeader("Authorization", access_token);
                randomCardRequest.downloadHandler = new DownloadHandlerBuffer();

                await randomCardRequest.SendWebRequest();

                if (randomCardRequest.isNetworkError || randomCardRequest.isHttpError)
                    throw new ShopException($"Error buying random card: {randomCardRequest.error}! Try later!");

                return int.Parse(randomCardRequest.downloadHandler.text);
            }
        }

        public async UniTask PostBuyGoldRequest(string access_token, int value)
        {
            string jsonBody = JsonConvert.SerializeObject(new { amount = value });

            using (var paymentRequest = new UnityWebRequest(_paymentEndpoint, "POST"))
            {
                paymentRequest.SetRequestHeader("Authorization", access_token);
                paymentRequest.SetRequestHeader("Content-Type", "application/json");
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
                paymentRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                paymentRequest.downloadHandler = new DownloadHandlerBuffer();

                await paymentRequest.SendWebRequest();

                if (paymentRequest.isNetworkError || paymentRequest.isHttpError)
                    throw new PaymentException($"Error doing payment: {paymentRequest.error}! Try later!");
            }
        }
    }
}
