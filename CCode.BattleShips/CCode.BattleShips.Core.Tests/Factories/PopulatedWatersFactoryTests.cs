using System;
using System.Collections.Generic;
using CCode.BattleShips.Core.DTO;
using CCode.BattleShips.Core.Factories;
using CCode.BattleShips.Core.Generators;
using CCode.BattleShips.Core.Validators;
using NUnit.Framework;
using Shouldly;

namespace CCode.BattleShips.Core.Tests.Factories
{
    public class PopulatedWatersFactoryTests
    {
        private const int ShipSize = 2;
        private const string ShipName = "Boat";
        private readonly List<ShipDefinition> _shipDefinitions = new() {new ShipDefinition {Name = ShipName, Size = ShipSize}};

        [Test]
        public void CreateWaterWithRandomlyPlacedShips_GivenOneShip_ReturnsWatersWithOneRandomlyPlacedShip()
        {
            var populatedWatersFactory = new PopulatedWatersFactory(new ShipFactory(new ShipLayoutValidator()), new CoordinateFactory(new Random()));
            var waters = populatedWatersFactory.CreateWaterWithRandomlyPlacedShips(_shipDefinitions);
            waters.Ships.Count.ShouldBeEquivalentTo(1);
            waters.Ships[0].Name.ShouldBeEquivalentTo(ShipName);
            waters.Ships[0].Coordinates.Count.ShouldBeEquivalentTo(ShipSize);
            waters.Ships[0].Hits.Count.ShouldBeEquivalentTo(0);
        }
    }
}