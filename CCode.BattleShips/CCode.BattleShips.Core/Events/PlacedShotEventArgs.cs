using System;
using CCode.BattleShips.Core.DTO;
using CCode.BattleShips.Core.Enums;

namespace CCode.BattleShips.Core.Events
{
    public class PlacedShotEventArgs : EventArgs
    {
        public PlayerType Player { get; }
        public ShotResult ShotResult { get; }

        public PlacedShotEventArgs(GameState currentGameState, ShotResult shotResult)
        {
            ShotResult = shotResult;
            Player = currentGameState.Player;
        }
    }
}