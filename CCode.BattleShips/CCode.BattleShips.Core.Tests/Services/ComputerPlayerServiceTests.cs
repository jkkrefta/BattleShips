using System;
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
    public class ComputerPlayerServiceTests
    {
        [Test]
        public void PlaceShot_TriggersOnProperGameState()
        {
            var gameService = new GameService(new GameFactory(
                new PopulatedWatersFactory(new ShipFactory(new ShipLayoutValidator()),
                    new CoordinateFactory(new Random()))));
            new ComputerPlayerService(gameService, new Random());
            var placedShot = false;
            gameService.PlacedShot += _ => placedShot = true;
            gameService.Game = new Game();
            gameService.SetNextGameState(new GameState(GamePhase.PlayersAction, PlayerType.ComputerPlayer));
            placedShot.ShouldBeTrue();
        }
    }
}