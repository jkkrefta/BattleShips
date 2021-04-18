using System.Collections.Generic;
using CCode.BattleShips.Core.Exceptions;
using NUnit.Framework;
using Shouldly;

namespace CCode.BattleShips.Core.Tests
{
    public class ShipTests
    {
        private Ship _ship;
        private readonly Coordinate _j10 = new("J10");
        private readonly Coordinate _c2 = new("C2");
        private readonly Coordinate _c3 = new("C3");
        private readonly Coordinate _c4 = new("C4");
        private readonly Coordinate _c5 = new("C5");

        [SetUp]
        public void Setup()
        {
            _ship = new Ship(ShipType.Destroyer, new List<Coordinate> {_c2, _c3, _c4, _c5});
        }
        
        [Test]
        public void IsWithinShipCoordinates_GivenCoordinateThatDoesNotMatchShipCoordinates_ReturnsFalse()
        {
            _ship.IsWithinShipCoordinates(_j10).ShouldBeFalse();
        }
        
        [Test]
        public void IsWithinShipCoordinates_GivenCoordinateThatDoesMatchShipCoordinates_ReturnsTrue()
        {
            _ship.IsWithinShipCoordinates(_c4).ShouldBeTrue();
        }
        
        [Test]
        public void WasHit_GivenCoordinateThatDoesNotMatchShipCoordinates_ReturnsFalse()
        {
            _ship.WasHit(_j10).ShouldBeFalse();
        }
        
        [Test]
        public void WasHit_GivenCoordinateThatDoesMatchShipCoordinates_ReturnsTrue()
        {
            _ship.RegisterHit(_c4);
            _ship.WasHit(_c4).ShouldBeTrue();
        }
        
        [Test]
        public void RegisterHit_GivenCoordinateThatDoesNotMatchShipCoordinates_Throws()
        {
            Should.Throw<InvalidHitCoordinateException>(() => _ship.RegisterHit(_j10));
        }
        
        [Test]
        public void RegisterHit_GivenAlreadyHitCoordinate_Throws()
        {
            _ship.RegisterHit(_c4);
            Should.Throw<AlreadyHitCoordinateException>(() => _ship.RegisterHit(_c4));
        }
        
        [Test]
        public void RegisterHit_GivenCoordinateWithinShipBounds_RegistersHit()
        {
            _ship.RegisterHit(_c4);
            _ship.WasHit(_c4).ShouldBeTrue();
        }
        
        [Test]
        public void IsSunk_WhenNoHitsOccured_ReturnsFalse()
        {
            _ship.IsSunk.ShouldBeFalse();
        }
        
        [Test]
        public void IsSunk_WhenSomeHitsOccured_ReturnsFalse()
        {
            _ship.RegisterHit(_c4);
            _ship.IsSunk.ShouldBeFalse();
        }
        
        [Test]
        public void IsSunk_WhenEveryShipCoordinateWasHits_ReturnsTrue()
        {
            _ship.RegisterHit(_c2);
            _ship.RegisterHit(_c3);
            _ship.RegisterHit(_c4);
            _ship.RegisterHit(_c5);
            _ship.IsSunk.ShouldBeTrue();
        }
    }
}