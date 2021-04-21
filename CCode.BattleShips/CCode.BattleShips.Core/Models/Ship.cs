using System.Collections.Generic;
using CCode.BattleShips.Core.Enums;
using CCode.BattleShips.Core.Exceptions;

namespace CCode.BattleShips.Core.Models
{
    public class Ship
    {
        public ShipType Type { get; }
        public List<Coordinate> Coordinates { get; }
        public List<Coordinate> Hits { get; }
        public bool IsSunk => Coordinates.Count == Hits.Count;

        public Ship(ShipType type, List<Coordinate> coordinates)
        {
            Type = type;
            Coordinates = coordinates;
            Hits = new List<Coordinate>();
        }
        
        public bool TryHit(Coordinate coordinate)
        {
            if (!IsWithinShipCoordinates(coordinate) || HasHitOn(coordinate)) return false;
            RegisterHit(coordinate);
            return true;
        }
        
        public bool IsWithinShipCoordinates(Coordinate coordinate)
        {
            return Coordinates.Exists(x => x.Equals(coordinate));
        }
        
        public bool HasHitOn(Coordinate coordinate)
        {
            return Hits.Exists(x => x.Equals(coordinate));
        }

        public void RegisterHit(Coordinate coordinate)
        {
            ValidateThatIsWithinShipCoordinates(coordinate);
            ValidateThatSamePlaceIsNotHitTwice(coordinate);
            Hits.Add(coordinate);
        }

        private void ValidateThatIsWithinShipCoordinates(Coordinate coordinate)
        {
            if (!Coordinates.Exists(x => x.Equals(coordinate)))
            {
                throw new InvalidHitCoordinateException(
                    $"Cannot register hit on {coordinate}. Ship coordinates are {string.Join(",", Coordinates)}");
            }
        }

        private void ValidateThatSamePlaceIsNotHitTwice(Coordinate coordinate)
        {
            if (Hits.Exists(x => x.Equals(coordinate)))
            {
                throw new AlreadyHitCoordinateException($"Cannot register hit on {coordinate}. It was already hit");
            }
        }
    }
}