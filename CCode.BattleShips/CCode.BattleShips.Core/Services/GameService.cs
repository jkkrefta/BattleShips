using System;
using CCode.BattleShips.Core.DTO;
using CCode.BattleShips.Core.Enums;
using CCode.BattleShips.Core.Events;
using CCode.BattleShips.Core.Factories;
using CCode.BattleShips.Core.Models;

namespace CCode.BattleShips.Core.Services
{
    public interface IGameService
    {
        event Action<PlacedShotEventArgs> PlacedShot;
        event Action<GameStateChangedEventArgs> GameStateChanged;
        event Action<GameStateChangedEventArgs> GameEnded;
        Game Game { get; }
        PlayerType CurrentPlayer { get; }
        void CreateNewGame();
        void StartGame();
        ShotResult PlaceShot(Coordinate target);
        bool CoordinateContainsShip(PlayerType player, Coordinate coordinate);
    }

    public class GameService : IGameService
    {
        public Game Game { get; private set; }
        private readonly IGameFactory _gameFactory;
        public event Action<PlacedShotEventArgs> PlacedShot;
        public event Action<GameStateChangedEventArgs> GameStateChanged;
        public event Action<GameStateChangedEventArgs> GameEnded;

        public GameService(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public PlayerType CurrentPlayer => Game.CurrentState.Player;

        public void CreateNewGame()
        {
            Game = _gameFactory.Create();
        }

        public void StartGame()
        {
            if (Game.CurrentState.Phase == GamePhase.Initializing)
            {
                SetNextGameState(new GameState(GamePhase.PlayersAction, PlayerType.HumanPlayer));
            }
        }

        public ShotResult PlaceShot(Coordinate target)
        {
            var shotResult = Game.PlaceShoot(target);
            OnPlacedShot(new PlacedShotEventArgs(Game.CurrentState, shotResult));
            SetNextGameState(Game.GetNextState(shotResult));
            return shotResult;
        }

        public bool CoordinateContainsShip(PlayerType player, Coordinate coordinate)
        {
            return Game.CoordinateContainsShip(player, coordinate);
        }

        private void SetNextGameState(GameState gameState)
        {
            Game.CurrentState = gameState;
            OnGameStateChanged(new GameStateChangedEventArgs(gameState));
            if (gameState.Phase == GamePhase.Victory)
            {
                OnGameEnded(new GameStateChangedEventArgs(gameState));
            }
        }

        protected virtual void OnPlacedShot(PlacedShotEventArgs obj) => PlacedShot?.Invoke(obj);
        protected virtual void OnGameStateChanged(GameStateChangedEventArgs obj) => GameStateChanged?.Invoke(obj);
        protected virtual void OnGameEnded(GameStateChangedEventArgs obj) => GameEnded?.Invoke(obj);
    }
}