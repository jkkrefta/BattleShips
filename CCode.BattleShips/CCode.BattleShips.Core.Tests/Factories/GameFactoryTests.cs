using System;
using CCode.BattleShips.Core.Enums;
using CCode.BattleShips.Core.Factories;
using CCode.BattleShips.Core.Generators;
using CCode.BattleShips.Core.Validators;
using NUnit.Framework;
using Shouldly;

namespace CCode.BattleShips.Core.Tests.Factories
{
    public class GameFactoryTests
    {
        [Test]
        public void Create_ReturnsNewPresetGame()
        {
            var gameFactory = new GameFactory(new PopulatedWatersFactory(new ShipFactory(new ShipLayoutValidator()),
                new CoordinateFactory(new Random())));
            var game = gameFactory.Create();
            game.HumanWaters.Ships.Count.ShouldBeEquivalentTo(3);
            game.ComputerWaters.Ships.Count.ShouldBeEquivalentTo(3);
            game.CurrentState.Phase.ShouldBeEquivalentTo(GamePhase.Initializing);
            game.CurrentState.Player.ShouldBeEquivalentTo(PlayerType.None);
        }
    }
}