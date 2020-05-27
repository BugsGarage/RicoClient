namespace RicoClient.Configs
{
    public class CardsConfig
    {
        public string AllCardsEndpoint { get; }

        public CardsConfig(AppConfig config)
        {
            AllCardsEndpoint = config.AllCardsEndpoint;
        }
    }
}
