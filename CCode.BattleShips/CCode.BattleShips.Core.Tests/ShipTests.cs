using System.Collections.Generic;
using NUnit.Framework;
using Shouldly;

namespace CCode.BattleShips.Core.Tests
{
    public class ShipTests
    {
        private Ship _ship;
        private readonly Coordinate _j10 = new("J10");
        private readonly Coordinate _c4 = new("C4");

        [SetUp]
        public void Setup()
        {
            _ship = new Ship(ShipType.Destroyer, new List<Coordinate>
            {
                new("C2"),
                new("C3"),
                _c4,
                new("C5")
            });
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
    }
}