using System;
using System.Collections.Generic;
using System.Linq;
using CCode.BattleShips.Core.Generators;
using CCode.BattleShips.Core.Models;
using CCode.BattleShips.Core.Validators;
using NUnit.Framework;
using Shouldly;

namespace CCode.BattleShips.Core.Tests.Generators
{
    public class CoordinateFactoryTests
    {
        private const int ShipSize = 5;
        private CoordinateFactory _shipPlacementService;
        private ShipLayoutValidator _shipLayoutValidator;

        [SetUp]
        public void SetUp()
        {
            _shipPlacementService = new CoordinateFactory(new Random());
            _shipLayoutValidator = new ShipLayoutValidator();
        }

        [Test]
        public void CreateRandomHorizontalLine_GivenShipSizeOf5_Returns()
        {
            
            var coordinates = _shipPlacementService.CreateRandomHorizontalLine(ShipSize).ToList();
            ThrowIfNotContinousLineOfSpecifiedSize(coordinates);
            coordinates.Any(x => x.Y != coordinates[0].Y).ShouldBeFalse();
        }

        [Test]
        public void CreateRandomVerticalLine_GivenShipSizeOf5_Returns()
        {
            var coordinates = _shipPlacementService.CreateRandomVerticalLine(ShipSize).ToList();
            ThrowIfNotContinousLineOfSpecifiedSize(coordinates);
            coordinates.Any(x => x.X != coordinates[0].X).ShouldBeFalse();
        }

        [Test]
        public void CreateRandomLine_GivenShipSizeOf5_Returns()
        {
            var coordinates = _shipPlacementService.CreateRandomLine(ShipSize).ToList();
            ThrowIfNotContinousLineOfSpecifiedSize(coordinates);
        }

        private void ThrowIfNotContinousLineOfSpecifiedSize(List<Coordinate> coordinates)
        {
            _shipLayoutValidator.ValidateShipLayout(coordinates);
            _shipLayoutValidator.ValidateShipSize(coordinates, ShipSize);
        }
    }
}