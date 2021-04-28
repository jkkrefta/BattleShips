using System;
using System.Collections.Generic;
using System.Linq;
using CCode.BattleShips.Core.DTO;
using CCode.BattleShips.Core.Exceptions;

namespace CCode.BattleShips.Core.Models
{
    public class Waters
    {
        public List<Ship> Ships { get; } = new();
        public List<Coordinate> MissedShots { get; } = new();

        public bool CanPlaceShot(Coordinate coordinate)
        {
            if (coordinate == null) return false;
            var ship = FirstOrDefaultShipOnCoordinates(coordinate);
            return !MissedShotExists(coordinate) && (ship == null || !ship.HasHitOn(coordinate));
        }
        
        public bool CanPlaceShip(Ship ship)
        {
            if (ship == null) return false;
            return !MissedShots.Any() && NoShipsOnWatersCollide(ship);
        }

        public ShotResult PlaceShot(Coordinate coordinate)
        {
            var hitShip = FirstOrDefaultShipOnCoordinates(coordinate);
            if (hitShip != null)
            {
                hitShip.RegisterHit(coordinate);
                return hitShip.IsSunk ? ShotResult.CreateSunkShip(coordinate, hitShip) : ShotResult.CreateHit(coordinate, hitShip);
            }
            ThrowIfSamePlaceIsHitTwice(coordinate);
            MissedShots.Add(coordinate);
            return ShotResult.CreateMiss(coordinate);
        }
        
        public void PlaceShip(Ship ship)
        {
            ThrowIfNull(ship);
            ThrowIfAnyShipCollides(ship);
            Ships.Add(ship);
        }

        private static void ThrowIfNull(Ship ship)
        {
            if (ship == null)
                throw new ArgumentNullException(nameof(ship));
        }

        private void ThrowIfAnyShipCollides(Ship ship)
        {
            ship.Coordinates.ForEach(coordinate =>
            {
                Ships.ForEach(shipOnWaters =>
                {
                    ThrowIfTwoShipsCollide(ship, shipOnWaters, coordinate);
                });
            });
        }

        private static void ThrowIfTwoShipsCollide(Ship ship, Ship shipOnWaters, Coordinate coordinate)
        {
            if (shipOnWaters.IsWithinShipCoordinates(coordinate))
                throw new InvalidShipPlacementException(
                    $"Cannot place ship on {string.Join(',', ship.Coordinates)}. There is collision on {coordinate}");
        }

        private void ThrowIfSamePlaceIsHitTwice(Coordinate coordinate)
        {
            if (MissedShotExists(coordinate))
                throw new AlreadyHitCoordinateException($"Cannot register hit on {coordinate}. It was already hit.");
        }

        private Ship? FirstOrDefaultShipOnCoordinates(Coordinate coordinate)
        {
            return Ships.FirstOrDefault(x => x.IsWithinShipCoordinates(coordinate));
        }
        
        private bool NoShipsOnWatersCollide(Ship ship)
        {
            var result = false;
            ship.Coordinates.ForEach(coordinate =>
            {
                if (Ships.FirstOrDefault(shipOnWaters => TwoShipsDoCollide(ship, shipOnWaters, coordinate)) == null)
                {
                    result = true;
                }
            });
            return result;
        }

        private static bool TwoShipsDoCollide(Ship ship, Ship shipOnWaters, Coordinate coordinate)
        {
            return shipOnWaters.IsWithinShipCoordinates(coordinate);
        }

        private bool MissedShotExists(Coordinate coordinate)
        {
            return MissedShots.Exists(x => x.Equals(coordinate));
        }
    }
}