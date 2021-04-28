using System.Collections.Generic;
using CCode.BattleShips.Core.Exceptions;

namespace CCode.BattleShips.Core.Models
{
    public class Ship
    {
        public string Name { get; }
        public List<Coordinate> Coordinates { get; }
        public List<Coordinate> Hits { get; }
        public bool IsSunk => Coordinates.Count == Hits.Count;

        public Ship(string name, List<Coordinate> coordinates)
        {
            ThrowIfNullOrEmpty(name);
            ThrowIfNullOrEmpty(coordinates);
            Name = name;
            Coordinates = coordinates;
            Hits = new List<Coordinate>();
        }

        public bool IsWithinShipCoordinates(Coordinate coordinate)
        {
            return Coordinates.Exists(shipCoordinate => shipCoordinate.Equals(coordinate));
        }

        public bool HasHitOn(Coordinate coordinate)
        {
            return Hits.Exists(hit => hit.Equals(coordinate));
        }

        public void RegisterHit(Coordinate coordinate)
        {
            ValidateThatIsWithinShipCoordinates(coordinate);
            ValidateThatSamePlaceIsNotHitTwice(coordinate);
            Hits.Add(coordinate);
        }

        private static void ThrowIfNullOrEmpty(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new InvalidShipConstructionException($"{nameof(name)} was null or empty");
        }

        private static void ThrowIfNullOrEmpty(List<Coordinate> coordinates)
        {
            if (coordinates == null || coordinates.Count == 0)
                throw new InvalidShipConstructionException($"{nameof(coordinates)} ware null or empty");
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
                throw new AlreadyHitCoordinateException($"Cannot register hit on {coordinate}. It was already hit {string.Join(',', Hits)}");
            }
        }
    }
}