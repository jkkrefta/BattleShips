using CCode.BattleShips.Core.Enums;

namespace CCode.BattleShips.Core.DTO
{
    public record GameState
    {
        public PlayerType Player { get; }
        public GamePhase Phase { get; }

        public GameState(GamePhase phase, PlayerType player)
        {
            Phase = phase;
            Player = player;
        }
    }
}