using System.Collections.Generic;
using CCode.BattleShips.Core.Exceptions;
using NUnit.Framework;
using Shouldly;

namespace CCode.BattleShips.Core.Tests
{
    public class ShipLayoutValidatorTests
    {
        private readonly Coordinate _a1 = new("A1");
        private readonly Coordinate _a2 = new("A2");
        private readonly Coordinate _a3 = new("A3");
        private readonly Coordinate _a5 = new("A5");
        private readonly Coordinate _b1 = new("B1");
        private readonly Coordinate _c1 = new("C1");
        private readonly Coordinate _j1 = new("J1");
        private readonly ShipLayoutValidator _shipLayoutValidator = new();

        [Test]
        public void ValidateShipLayout_GivenNonContinousVerticalLine_Throws()
        {
            Should.Throw<InvalidCoordinateException>(() =>
                _shipLayoutValidator.ValidateShipLayout(new List<Coordinate> {_a1, _a2, _a5}));
        }
        
        [Test]
        public void ValidateShipLayout_GivenContinousVerticalLine_DoesNothing()
        {
            _shipLayoutValidator.ValidateShipLayout(new List<Coordinate> {_a1, _a2, _a3});
        }

        [Test]
        public void ValidateShipLayout_GivenNonContinousHorizontalLine_Throws()
        {
            Should.Throw<InvalidCoordinateException>(() =>
                _shipLayoutValidator.ValidateShipLayout(new List<Coordinate> {_a1, _b1, _j1}));
        }
        
        [Test]
        public void ValidateShipLayout_GivenContinousHorizontalLine_DoesNothing()
        {
            _shipLayoutValidator.ValidateShipLayout(new List<Coordinate> {_a1, _b1, _c1});
        }
        
        [Test]
        public void ValidateShipSize_GivenThreeTileShipAndShipSizeOf4_Throws()
        {
            Should.Throw<InvalidCoordinateException>(() =>
                _shipLayoutValidator.ValidateShipSize(new List<Coordinate> {_a1, _b1, _c1}, 4));
        }
        
        [Test]
        public void ValidateShipSize_GivenThreeTileShipAndShipSizeOf3_DoesNothing()
        {
            _shipLayoutValidator.ValidateShipSize(new List<Coordinate> {_a1, _b1, _c1}, 3);
        }
    }
}