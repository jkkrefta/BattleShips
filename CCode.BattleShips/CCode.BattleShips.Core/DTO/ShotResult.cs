using CCode.BattleShips.Core.Enums;
using CCode.BattleShips.Core.Models;

namespace CCode.BattleShips.Core.DTO
{
    public class ShotResult
    {
        public HitType HitType { get; }
        public Ship HitShip { get; }
        public Coordinate Coordinate { get; }

        private ShotResult(Coordinate coordinate, HitType hitType, Ship hitShip = null)
        {
            Coordinate = coordinate;
            HitType = hitType;
            HitShip = hitShip;
        }

        public static ShotResult CreateMiss(Coordinate coordinate) => new ShotResult(coordinate, HitType.Miss);
        public static ShotResult CreateHit(Coordinate coordinate, Ship hitShip) => new ShotResult(coordinate, HitType.Hit, hitShip);
        public static ShotResult CreateSunkShip(Coordinate coordinate, Ship hitShip) => new ShotResult(coordinate, HitType.SunkShip, hitShip);
    }
}