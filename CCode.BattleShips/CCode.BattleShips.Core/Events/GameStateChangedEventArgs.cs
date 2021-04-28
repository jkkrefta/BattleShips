using System;
using CCode.BattleShips.Core.DTO;
using CCode.BattleShips.Core.Enums;

namespace CCode.BattleShips.Core.Events
{
    public class GameStateChangedEventArgs : EventArgs
    {
        public GamePhase Phase { get; }
        public PlayerType Player { get; }

        public GameStateChangedEventArgs(GameState currentGameState)
        {
            Phase = currentGameState.Phase;
            Player = currentGameState.Player;
        }    
    }
}