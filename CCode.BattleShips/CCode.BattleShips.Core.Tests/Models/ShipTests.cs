using System.Collections.Generic;
using CCode.BattleShips.Core.Exceptions;
using CCode.BattleShips.Core.Models;
using NUnit.Framework;
using Shouldly;

namespace CCode.BattleShips.Core.Tests.Models
{
    public class ShipTests
    {
        private Ship _ship;
        private readonly Coordinate _j10 = new("J10");
        private readonly Coordinate _c2 = new("C2");
        private readonly Coordinate _c3 = new("C3");
        private readonly Coordinate _c4 = new("C4");
        private readonly Coordinate _c5 = new("C5");
        private readonly string _dropship = "Dropship";

        [SetUp]
        public void Setup()
        {
            _ship = new Ship(_dropship, new List<Coordinate> {_c2, _c3, _c4, _c5});
        }

        [Test]
        public void Ship_GivenNullName_Throws()
        {
            Should.Throw<InvalidShipConstructionException>(() => new Ship(null, null));
        }
        
        [Test]
        public void Ship_GivenEmptyName_Throws()
        {
            Should.Throw<InvalidShipConstructionException>(() => new Ship(string.Empty, null));
        }
        
        [Test]
        public void Ship_GivenNullCoordinates_Throws()
        {
            Should.Throw<InvalidShipConstructionException>(() => new Ship(_dropship, null));
        }
        
        [Test]
        public void Ship_GivenEmptyCoordinates_Throws()
        {
            Should.Throw<InvalidShipConstructionException>(() => new Ship(_dropship, new List<Coordinate>()));
        }
        
        [Test]
        public void Ship_GivenNameAndCoordinates_CreatesInstance()
        {
            var coordinates = new List<Coordinate>{_c2};
            var ship = new Ship(_dropship, coordinates);
            ship.Name.ShouldBe(_dropship);
            ship.Coordinates.ShouldBe(coordinates);
            ship.Hits.ShouldBeEmpty();
            ship.IsSunk.ShouldBeFalse();
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
        public void HasHitOn_GivenCoordinateThatDoesNotMatchShipCoordinates_ReturnsFalse()
        {
            _ship.HasHitOn(_j10).ShouldBeFalse();
        }
        
        [Test]
        public void HasHitOn_GivenCoordinateThatDoesMatchShipCoordinates_ReturnsTrue()
        {
            _ship.RegisterHit(_c4);
            _ship.HasHitOn(_c4).ShouldBeTrue();
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
            _ship.HasHitOn(_c4).ShouldBeTrue();
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