using System;
using System.Collections.Generic;
using CCode.BattleShips.Core.Enums;
using CCode.BattleShips.Core.Exceptions;
using CCode.BattleShips.Core.Models;
using NUnit.Framework;
using Shouldly;

namespace CCode.BattleShips.Core.Tests
{
    public class WatersTests
    {
        private Waters _waters;
        private static readonly Coordinate A1 = new("A1");
        private static readonly Coordinate A2 = new("A2");
        private static readonly Coordinate J10 = new("J10");
        private readonly Ship _shipA1A2 = new(ShipType.Undefined, new List<Coordinate> {A1, A2});
        private readonly Ship _shipJ10 = new(ShipType.Undefined, new List<Coordinate> {J10});

        [SetUp]
        public void Setup()
        {
            _waters = new Waters();
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
        public void PlaceShot_WhenGivenCoordinatesOnThatAreHit_ReturnsMiss()
        {
            _waters.PlaceShip(_shipA1A2);
            var shootResult = _waters.PlaceShot(A1);
            shootResult.HitType.ShouldBe(HitType.Hit);
            _waters.MissedShots.Count.ShouldBe(0);
        }
        
        [Test]
        public void PlaceShot_WhenGivenCoordinatesOnThatWillKillShip_ReturnsSunkShip()
        {
            _waters.PlaceShip(_shipA1A2);
            _waters.PlaceShot(A1);
            var shootResult = _waters.PlaceShot(A2);
            shootResult.HitType.ShouldBe(HitType.SunkShip);
            _waters.MissedShots.Count.ShouldBe(0);
        }
    }
}