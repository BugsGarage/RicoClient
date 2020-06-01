namespace RicoClient.Scripts.Network.Entities.Websocket
{
    public enum RequestCommandType
    {
        Connect,
        Ping,
        Disconnect,
        GameStartReady,
        EndTurnReady,
        CardPlayRequest
    }

    public enum ResponseCommandType
    {
        Error,
        Ping,
        Started,
        Finished,
        PlayersTurnFinsh,
        EnemyTurnFinsh,
        PlayersTurnStart,
        EnemyTurnStart,
        CardPlayResponse,
        EnemyCardPlayResponse
    }
}
