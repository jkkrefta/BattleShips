namespace CCode.BattleShips.Core.Enums
{
    public enum GamePhase
    {
        Undefined,
        Initializing,
        PlacingShips,
        PlayersAction,
        EvaluatingPlayersAction,
        CheckingVictoryConditions,
        Victory,
        GameEnded
    }
}