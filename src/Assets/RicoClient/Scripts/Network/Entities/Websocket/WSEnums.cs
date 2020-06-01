namespace RicoClient.Scripts.Network.Entities.Websocket
{
    public enum RequestCommandType
    {
        Connect,
        Ping,
        Disconnect,
        GameStartReady,
        EndTurnReady,
        CardPlayRequest,
        AbilityUseRequest,
        AttackRequest
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
        EnemyCardPlayResponse,
        CanUseAbilityResponse,
        UseAbilityResponse,
        EnemyUseAbilityResponse,
        AttackResponse,
        EnemyAttackResponse
    }
}
