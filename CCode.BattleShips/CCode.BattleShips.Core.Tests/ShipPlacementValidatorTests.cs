using System.Collections.Generic;
using CCode.BattleShips.Core.Enums;
using CCode.BattleShips.Core.Exceptions;
using CCode.BattleShips.Core.Models;
using CCode.BattleShips.Core.Validators;
using NUnit.Framework;
using Shouldly;

namespace CCode.BattleShips.Core.Tests
{
    public class ShipPlacementValidatorTests
    {
        private readonly ShipPlacementValidator _shipPlacementValidator = new();
        private readonly Ship _shipA1 = new(ShipType.Undefined, new List<Coordinate> {new("A1")});
        private readonly Ship _shipJ10 = new(ShipType.Undefined, new List<Coordinate> {new("J10")});

        [Test]
        public void ValidateShipPlacement_GivenWatersContainingShipThatColidesWithGivenShip_Throws()
        {
            var waters = new Waters();
            waters.Ships.Add(_shipA1);
            Should.Throw<InvalidShipPlacementException>(
                () => _shipPlacementValidator.ValidateShipPlacement(waters, _shipA1));
        }
        
        [Test]
        public void ValidateShipPlacement_GivenWatersContainingShipThatDoesNotColideWithGivenShip_DoesNothing()
        {
            var waters = new Waters();
            waters.Ships.Add(_shipA1);
            _shipPlacementValidator.ValidateShipPlacement(waters, _shipJ10);
        }
    }
}