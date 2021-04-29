using System;
using System.Collections.Generic;
using CCode.BattleShips.Core.DTO;
using CCode.BattleShips.Core.Enums;
using CCode.BattleShips.Core.Factories;
using CCode.BattleShips.Core.Generators;
using CCode.BattleShips.Core.Models;
using CCode.BattleShips.Core.Services;
using CCode.BattleShips.Core.Validators;
using NUnit.Framework;
using Shouldly;

namespace CCode.BattleShips.Core.Tests.Services
{
    public class GameServiceTests
    {
        private GameService _gameService;
        private static readonly Coordinate A1 = new("A1");
        private readonly Ship _ship = new("Sink me", new List<Coordinate>(new List<Coordinate>{A1}));

        [SetUp]
        public void Setup()
        {
            _gameService = new GameService(new GameFactory(new PopulatedWatersFactory(new ShipFactory(new ShipLayoutValidator()),
                new CoordinateFactory(new Random()))));
        }
        
        [Test]
        public void CreateNewGame_CreatesInstanceOfGame()
        {
            _gameService.CreateNewGame();
            _gameService.Game.ShouldNotBeNull();
        }
        
        [Test]
        public void StartGame_WhenGameIsNotStarted_Throws()
        {
            Should.Throw<NullReferenceException>(() => _gameService.StartGame());
        }
        
        [Test]
        public void StartGame_SetsStateToHumanPlayerAction()
        {
            _gameService.CreateNewGame();
            _gameService.StartGame();
            _gameService.Game.CurrentState.Phase.ShouldBe(GamePhase.PlayersAction);
            _gameService.Game.CurrentState.Player.ShouldBe(PlayerType.HumanPlayer);
        }
        
        [Test]
        public void StartGame_WhenStateIsNotInitializing_DoesNothing()
        {
            var gameState = new GameState(GamePhase.Victory, PlayerType.None);
            _gameService.CreateNewGame();
            _gameService.SetNextGameState(gameState);
            _gameService.StartGame();
            _gameService.Game.CurrentState.ShouldBe(gameState);
        }
        
        [Test]
        public void PlaceShot_ReturnsShotResult()
        {
            _gameService.CreateNewGame();
            _gameService.StartGame();
            var shotResult = _gameService.PlaceShot(A1);
            shotResult.ShouldNotBeNull();
            shotResult.HitType.ShouldBe(HitType.Miss);
        }
        
        [Test]
        public void PlaceShot_SetsNextGameState()
        {
            _gameService.CreateNewGame();
            _gameService.StartGame();
            _gameService.PlaceShot(A1);
            _gameService.Game.CurrentState.Phase.ShouldBe(GamePhase.PlayersAction);
            _gameService.Game.CurrentState.Player.ShouldBe(PlayerType.ComputerPlayer);
        }
        
        [Test]
        public void PlaceShot_PublishesEventOnPlacedShot()
        {
            var hasBeenInvoked = false;
            _gameService.PlacedShot += args => hasBeenInvoked = true;
            _gameService.CreateNewGame();
            _gameService.StartGame();
            _gameService.PlaceShot(A1);
            hasBeenInvoked.ShouldBeTrue();
        }
        
        [Test]
        public void PlaceShot_PublishesEventGameStateChanged()
        {
            var hasBeenInvoked = false;
            _gameService.GameStateChanged += args => hasBeenInvoked = true;
            _gameService.CreateNewGame();
            _gameService.StartGame();
            _gameService.PlaceShot(A1);
            hasBeenInvoked.ShouldBeTrue();
        }
        
        [Test]
        public void PlaceShot_WhenShotTriggersVictory_PublishesEventGameEnded()
        {
            var hasBeenInvoked = false;
            _gameService.GameEnded += args => hasBeenInvoked = true;
            _gameService.Game = new Game
            {
                CurrentState = new GameState(GamePhase.PlayersAction, PlayerType.HumanPlayer)
            };
            _gameService.Game.ComputerWaters.PlaceShip(_ship);
            _gameService.StartGame();
            _gameService.PlaceShot(A1);
            hasBeenInvoked.ShouldBeTrue();
        }

        [Test]
        public void CoordinateContainsShip_WhenThereIsNoShip_ReturnsFalse()
        {
            _gameService.Game = new Game();
            _gameService.CoordinateContainsShip(PlayerType.HumanPlayer, A1).ShouldBeFalse();
        }
        
        [Test]
        public void CoordinateContainsShip_WhenThereIsNoShip_ReturnsTrue()
        {
            _gameService.Game = new Game();
            _gameService.Game.HumanWaters.PlaceShip(_ship);
            _gameService.CoordinateContainsShip(PlayerType.HumanPlayer, A1).ShouldBeTrue();
        }
    }
}