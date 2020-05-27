namespace RicoClient.Configs
{
    public class PlayerConfig
    {
        public string EntranceEndpoint { get; }
        public string PlayerInfoEndpoint { get; }
        public string NewDeckEndpoint { get; }

        public PlayerConfig(AppConfig config)
        {
            EntranceEndpoint = config.EntranceEndpoint;
            PlayerInfoEndpoint = config.PlayerInfoEndpoint;
            NewDeckEndpoint = config.NewDeckEndpoint;
        }
    }
}
