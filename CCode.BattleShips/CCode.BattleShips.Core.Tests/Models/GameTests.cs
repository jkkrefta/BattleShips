using System.Collections.Generic;
using CCode.BattleShips.Core.DTO;
using CCode.BattleShips.Core.Enums;
using CCode.BattleShips.Core.Exceptions;
using CCode.BattleShips.Core.Models;
using NUnit.Framework;
using Shouldly;

namespace CCode.BattleShips.Core.Tests.Models
{
    public class GameTests
    {
        private Game _game;
        private Ship _a1A2Ship;
        private Ship _j10Ship;
        private static readonly Coordinate A1 = new("A1");
        private static readonly Coordinate A2 = new("A2");
        private static readonly Coordinate J10 = new("J10");
        

        [SetUp]
        public void Setup()
        {
            _game = new Game();
            _a1A2Ship = new Ship("Boat", new List<Coordinate> {A1, A2});
            _j10Ship = new Ship("Boat", new List<Coordinate> {J10});
        }

        [Test]
        public void PlaceShot_WhenPhaseIsInitializing_Throws()
        {
            Should.Throw<InvalidGameStateException>(() => _game.PlaceShoot(A1));
        }
        
        [Test]
        public void PlaceShot_WhenPhaseIsVictory_Throws()
        {
            _game.CurrentState = new GameState(GamePhase.Victory, PlayerType.ComputerPlayer);
            Should.Throw<InvalidGameStateException>(() => _game.PlaceShoot(A1));
        }
        
        [Test]
        public void PlaceShot_WhenPhaseIsPlayerActionAndPlayerTypeIsNone_Throws()
        {
            _game.CurrentState = new GameState(GamePhase.PlayersAction, PlayerType.None);
            Should.Throw<InvalidGameStateException>(() => _game.PlaceShoot(A1));
        }
        
        [Test]
        public void PlaceShot_WhenPhaseIsPlayerActionAndPlayerTypeIsComputer_PlacesShotOnHumanWaters()
        {
            _game.CurrentState = new GameState(GamePhase.PlayersAction, PlayerType.ComputerPlayer);
            _game.PlaceShoot(A1);
            _game.HumanWaters.CanPlaceShot(A1).ShouldBeFalse();
        }
        
        [Test]
        public void PlaceShot_WhenPhaseIsPlayerActionAndPlayerTypeIsHuman_PlacesShotOnComputerWaters()
        {
            _game.CurrentState = new GameState(GamePhase.PlayersAction, PlayerType.HumanPlayer);
            _game.PlaceShoot(A1);
            _game.ComputerWaters.CanPlaceShot(A1).ShouldBeFalse();
        }

        [Test]
        public void GetNextState_WhenCurrentStateIsInitializing_Throws()
        {
            Should.Throw<InvalidGameStateException>(() => _game.GetNextState(ShotResult.CreateMiss(A1)));
        }
        
        [Test]
        public void GetNextState_WhenCurrentStateIsVictory_Throws()
        {
            _game.CurrentState = new GameState(GamePhase.Victory, PlayerType.None);
            Should.Throw<InvalidGameStateException>(() => _game.GetNextState(ShotResult.CreateMiss(A1)));
        }
        
        [Test]
        public void GetNextState_WhenCurrentStateIsPlayersActionAndPlayerNone_Throws()
        {
            _game.CurrentState = new GameState(GamePhase.PlayersAction, PlayerType.None);
            Should.Throw<InvalidGameStateException>(() => _game.GetNextState(ShotResult.CreateMiss(A1)));
        }
        
        [Test]
        public void GetNextState_GivenMiss_WhenCurrentStateIsPlayersActionAndHumanPlayer_ReturnsPlayersAction()
        {
            _game.CurrentState = new GameState(GamePhase.PlayersAction, PlayerType.HumanPlayer);
            var result = _game.GetNextState(ShotResult.CreateMiss(A1));
            result.Phase.ShouldBe(GamePhase.PlayersAction);
            result.Player.ShouldBe(PlayerType.ComputerPlayer);
        }
        
        [Test]
        public void GetNextState_GivenMiss_WhenCurrentStateIsPlayersActionAndComputer_ReturnsPlayersAction()
        {
            _game.CurrentState = new GameState(GamePhase.PlayersAction, PlayerType.ComputerPlayer);
            var result = _game.GetNextState(ShotResult.CreateMiss(A1));
            result.Phase.ShouldBe(GamePhase.PlayersAction);
            result.Player.ShouldBe(PlayerType.HumanPlayer);
        }
        
        [Test]
        public void GetNextState_GivenHit_WhenCurrentStateIsPlayersActionAndComputer_ReturnsPlayersAction()
        {
            _game.CurrentState = new GameState(GamePhase.PlayersAction, PlayerType.ComputerPlayer);
            var result = _game.GetNextState(ShotResult.CreateHit(A1, _a1A2Ship));
            result.Phase.ShouldBe(GamePhase.PlayersAction);
            result.Player.ShouldBe(PlayerType.ComputerPlayer);
        }
        
        [Test]
        public void GetNextState_GivenSink_WhenCurrentStateIsPlayersActionAndComputerAndNotAllShipsAreSink_ReturnsPlayersAction()
        {
            _game.CurrentState = new GameState(GamePhase.PlayersAction, PlayerType.ComputerPlayer);
            _game.HumanWaters.PlaceShip(_a1A2Ship);
            _game.HumanWaters.PlaceShip(_j10Ship);
            var result = _game.GetNextState(_game.PlaceShoot(J10));
            result.Phase.ShouldBe(GamePhase.PlayersAction);
            result.Player.ShouldBe(PlayerType.ComputerPlayer);
        }
        
        [Test]
        public void GetNextState_GivenSink_WhenCurrentStateIsPlayersActionAndComputerAndAllShipsAreSink_ReturnsVictory()
        {
            _game.CurrentState = new GameState(GamePhase.PlayersAction, PlayerType.ComputerPlayer);
            _game.HumanWaters.PlaceShip(_j10Ship);
            var result = _game.GetNextState(_game.PlaceShoot(J10));
            result.Phase.ShouldBe(GamePhase.Victory);
            result.Player.ShouldBe(PlayerType.ComputerPlayer);
        }

        [Test]
        public void CoordinateContainsShip()
        {
            Should.Throw<InvalidGameStateException>(() => _game.CoordinateContainsShip(PlayerType.None, J10));
        }
        
        [Test]
        public void CoordinateContainsShip1()
        {
            _game.CoordinateContainsShip(PlayerType.HumanPlayer, J10).ShouldBeFalse();
        }
        
        [Test]
        public void CoordinateContainsShip2()
        {
            _game.HumanWaters.PlaceShip(_a1A2Ship);
            _game.CoordinateContainsShip(PlayerType.HumanPlayer, J10).ShouldBeFalse();
        }
        
        [Test]
        public void CoordinateContainsShip3()
        {
            _game.HumanWaters.PlaceShip(_j10Ship);
            _game.CoordinateContainsShip(PlayerType.HumanPlayer, J10).ShouldBeTrue();
        }
    }
}