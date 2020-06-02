using RicoClient.Configs;
using RicoClient.Scripts.Exceptions;
using UniRx.Async;
using UnityEngine.Networking;

namespace RicoClient.Scripts.Network.Controllers
{
    public class GameController
    {
        private readonly string _gameConnectEndpoint;

        public GameController(GameConfig configuration)
        {
            _gameConnectEndpoint = configuration.GameConnectEndpoint;
        }

        public async UniTask PostConnectGameRequest(string access_token, uint deckId)
        {
            string connectGameEndpoint = _gameConnectEndpoint + "/" + deckId.ToString();

            using (var connectGameRequest = new UnityWebRequest(connectGameEndpoint, "POST"))
            {
                connectGameRequest.SetRequestHeader("Authorization", access_token);
                connectGameRequest.downloadHandler = new DownloadHandlerBuffer();

                await connectGameRequest.SendWebRequest();

                if (connectGameRequest.isNetworkError || connectGameRequest.isHttpError)
                    throw new GameException($"Error during connecting game: {connectGameRequest.error}! Try later!");
            }
        }
    }
}
