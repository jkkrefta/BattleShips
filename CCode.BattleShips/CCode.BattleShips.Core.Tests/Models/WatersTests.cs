using System;
using System.Collections.Generic;
using CCode.BattleShips.Core.Enums;
using CCode.BattleShips.Core.Exceptions;
using CCode.BattleShips.Core.Models;
using NUnit.Framework;
using Shouldly;

namespace CCode.BattleShips.Core.Tests.Models
{
    public class WatersTests
    {
        private Waters _waters;
        private Ship _shipA1A2;
        private Ship _shipJ10;
        private static readonly Coordinate A1 = new("A1");
        private static readonly Coordinate A2 = new("A2");
        private static readonly Coordinate J10 = new("J10");

        [SetUp]
        public void Setup()
        {
            _waters = new Waters();
            _shipA1A2 = new Ship("A1A2 Ship", new List<Coordinate> {A1, A2});
            _shipJ10 = new Ship("J10 Ship", new List<Coordinate> {J10});
        }

        [Test]
        public void CanPlaceShot_WhenWatersAreEmpty_ReturnsTrue()
        {
            _waters.CanPlaceShot(A1).ShouldBeTrue();
        }
        
        [Test]
        public void CanPlaceShot_GivenCoordinateWatersAlreadyContainAsMiss_ReturnsFalse()
        {
            _waters.PlaceShot(A1);
            _waters.CanPlaceShot(A1).ShouldBeFalse();
        }
        
        [Test]
        public void CanPlaceShot_WhenWatersAlreadyContainGivenShotAsShip_ReturnsTrue()
        {
            _waters.PlaceShip(_shipA1A2);
            _waters.CanPlaceShot(A1).ShouldBeTrue();
        }
        
        [Test]
        public void CanPlaceShot_WhenWatersAlreadyContainGivenShotAsShipWithHit_ReturnsFalse()
        {
            _waters.PlaceShip(_shipA1A2);
            _waters.PlaceShot(A1);
            _waters.CanPlaceShot(A1).ShouldBeFalse();
        }

        [Test]
        public void CanPlaceShip_WhenWatersAreEmpty_ReturnsTrue()
        {
            _waters.CanPlaceShip(_shipJ10).ShouldBeTrue();
        }
        
        [Test]
        public void CanPlaceShip_WhenWatersContainAnyShot_ReturnsFalse()
        {
            _waters.PlaceShot(A1);
            _waters.CanPlaceShip(_shipJ10).ShouldBeFalse();
        }
        
        [Test]
        public void CanPlaceShip_WhenWatersAlreadyContainShipOnCoordinates_ReturnsFalse()
        {
            _waters.PlaceShip(_shipJ10);
            _waters.CanPlaceShip(_shipJ10).ShouldBeFalse();
        }
        
        [Test]
        public void CanPlaceShip_WhenWatersContainShipOtherShip_ReturnsFalse()
        {
            _waters.PlaceShip(_shipA1A2);
            _waters.CanPlaceShip(_shipJ10).ShouldBeTrue();
        }
        
        [Test]
        public void PlaceShip_GivenNull_Throws()
        {
            Should.Throw<ArgumentNullException>(() => _waters.PlaceShip(null));
        }
        
        [Test]
        public void PlaceShip_WhenContainingShipThatColidesWithGivenShip_Throws()
        {
            _waters.PlaceShip(_shipA1A2);
            Should.Throw<InvalidShipPlacementException>(() => _waters.PlaceShip(_shipA1A2));
        }
        
        [Test]
        public void PlaceShip_WhenContainingShipThatDoesNotColideWithGivenShip_AddsShipToShips()
        {
            _waters.PlaceShip(_shipA1A2);
            _waters.PlaceShip(_shipJ10);
            _waters.Ships.Count.ShouldBe(2);
        }
        
        [Test]
        public void PlaceShot_WhenGivenCoordinatesOnEmptyWaters_ReturnsMiss()
        {
            var shootResult = _waters.PlaceShot(A1);
            shootResult.HitType.ShouldBe(HitType.Miss);
            _waters.MissedShots.Count.ShouldBe(1);
        }
        
        [Test]
        public void PlaceShot_WhenTargetingSameMissTwice_Throws()
        {
            _waters.PlaceShot(A1);
            Should.Throw<AlreadyHitCoordinateException>(() => _waters.PlaceShot(A1));
        }
        
        [Test]
        public void PlaceShot_WhenGivenCoordinatesOnThatAreMiss_ReturnsMiss()
        {
            _waters.PlaceShip(_shipJ10);
            var shootResult = _waters.PlaceShot(A1);
            shootResult.HitType.ShouldBe(HitType.Miss);
            _waters.MissedShots.Count.ShouldBe(1);
        }
        
        [Test]
        public void PlaceShot_WhenGivenCoordinatesOnThatAreHit_ReturnsHit()
        {
            _waters.PlaceShip(_shipA1A2);
            Console.WriteLine(string.Join(",", _shipA1A2.Hits));
            var shootResult = _waters.PlaceShot(A1);
            shootResult.HitType.ShouldBe(HitType.Hit);
            _waters.MissedShots.Count.ShouldBe(0);
        }
        
        [Test]
        public void PlaceShot_WhenGivenCoordinatesOnThatWillKillShip_ReturnsSunkShip()
        {
            _waters.PlaceShip(_shipJ10);
            var shootResult = _waters.PlaceShot(J10);
            shootResult.HitType.ShouldBe(HitType.SunkShip);
            _waters.MissedShots.Count.ShouldBe(0);
        }
    }
}