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

        public void PlaceShip(Ship ship)
        {
            if (ship == null)
            {
                throw new ArgumentNullException(nameof(ship));
            }
            ValidateShipPlacement(ship);
            Ships.Add(ship);
        }

        public ShotResult PlaceShot(Coordinate shoot)
        {
            var hitShip = Ships.FirstOrDefault(x => x.TryHit(shoot));
            if (hitShip != null)
            {
                return hitShip.IsSunk ? ShotResult.CreateSunkShip(hitShip) : ShotResult.CreateHit(hitShip);
            }
            ValidateThatSamePlaceIsNotHitTwice(shoot);
            MissedShots.Add(shoot);
            return ShotResult.CreateMiss();
        }
        
        private void ValidateShipPlacement(Ship ship)
        {
            ship.Coordinates.ForEach(coordinate =>
            {
                Ships.ForEach(shipOnWaters => { ValidateThatShipsDoNotCollide(ship, shipOnWaters, coordinate); });
            });
        }

        private static void ValidateThatShipsDoNotCollide(Ship ship, Ship shipOnWaters, Coordinate coordinate)
        {
            if (shipOnWaters.IsWithinShipCoordinates(coordinate))
            {
                throw new InvalidShipPlacementException(
                    $"Cannot place ship on {string.Join(',', ship.Coordinates)}. There is collision on {coordinate}");
            }
        }
        
        private void ValidateThatSamePlaceIsNotHitTwice(Coordinate coordinate)
        {
            if (MissedShots.Exists(x => x.Equals(coordinate)))
            {
                throw new AlreadyHitCoordinateException($"Cannot register hit on {coordinate}. It was already hit");
            }
        }
    }
}