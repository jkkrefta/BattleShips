using System.Collections.Generic;
using CCode.BattleShips.Core.DTO;
using CCode.BattleShips.Core.Enums;
using CCode.BattleShips.Core.Exceptions;

namespace CCode.BattleShips.Core.Models
{
    public class Game
    {
        public Waters HumanPlayer;
        public Waters ComputerPlayer;
        public GameState CurrentGameState;
        public Queue<GameState> GameStateQueue;
        
        public void Initialize()
        {
            HumanPlayer = new Waters();
            ComputerPlayer = new Waters();
            CurrentGameState = new GameState(GamePhase.Initializing, PlayerType.None);
            GameStateQueue = new Queue<GameState>();
        }

        public GameState NextState() => CurrentGameState = GameStateQueue.Dequeue();

        public Waters GetCurrentPlayersWaters()
        {
            switch (CurrentGameState.Player)
            {
                case PlayerType.ComputerPlayer:
                    return ComputerPlayer;
                case PlayerType.HumanPlayer:
                    return HumanPlayer;
                default:
                    throw new InvalidGameStateException(
                        "Current game state has no designated player. Cannot retrieve current players waters");
            }
        }
        
        public Waters GetOtherPlayersWaters()
        {
            switch (CurrentGameState.Player)
            {
                case PlayerType.HumanPlayer:
                    return ComputerPlayer;
                case PlayerType.ComputerPlayer:
                    return HumanPlayer;
                default:
                    throw new InvalidGameStateException(
                        "Current game state has no designated player. Cannot retrieve current players waters");
            }
        }

        public void EnqueueNextPlayersAction()
        {
            if (CurrentGameState.Player == PlayerType.ComputerPlayer)
            {
                EnqueuePlayersAction(PlayerType.HumanPlayer);
            }

            if (CurrentGameState.Player == PlayerType.HumanPlayer)
            {
                EnqueuePlayersAction(PlayerType.ComputerPlayer);
            }
            
            throw new InvalidGameStateException(
                "Current game state has no designated player. Enqueue next players action");
        }

        public void EnqueuePlacingShipsForNextPlayer()
        {
            if (CurrentGameState.Player == PlayerType.ComputerPlayer)
            {
                EnqueueGameState(new GameState(GamePhase.PlacingShips, PlayerType.HumanPlayer));
            }

            if (CurrentGameState.Player == PlayerType.HumanPlayer)
            {
                EnqueueGameState(new GameState(GamePhase.PlacingShips, PlayerType.ComputerPlayer));
            }
            
            throw new InvalidGameStateException(
                "Current game state has no designated player. Enqueue next players ship placement");
        }


        public void EnqueuePlacingShipsForPlayer(PlayerType player) => EnqueueGameState(new GameState(GamePhase.PlacingShips, player));
        public void EnqueueCurrentPlayersAction() => EnqueuePlayersAction(CurrentGameState.Player);
        public void EnqueueEvaluatingCurrentPlayersAction() => EnqueueGameState(new GameState(GamePhase.EvaluatingPlayersAction, CurrentGameState.Player));
        public void EnqueueCheckingVictoryConditionsCurrentPlayersAction() => EnqueueGameState(new GameState(GamePhase.CheckingVictoryConditions, CurrentGameState.Player));
        public void EnqueueVictory() => EnqueueGameState(new GameState(GamePhase.Victory, CurrentGameState.Player));

        private void EnqueuePlayersAction(PlayerType player) => EnqueueGameState(new GameState(GamePhase.PlayersAction, player));
        private void EnqueueGameState(GameState gameState) => GameStateQueue.Enqueue(gameState);
    }
}