using CCode.BattleShips.Core.Enums;
using CCode.BattleShips.Core.Models;

namespace CCode.BattleShips.Core.DTO
{
    public class ShotResult
    {
        public HitType HitType { get; }
        public Ship HitShip { get; }

        private ShotResult(HitType hitType, Ship hitShip = null)
        {
            HitType = hitType;
            HitShip = hitShip;
        }

        public static ShotResult CreateMiss() => new ShotResult(HitType.Miss);
        public static ShotResult CreateHit(Ship hitShip) => new ShotResult(HitType.Hit, hitShip);
        public static ShotResult CreateSunkShip(Ship hitShip) => new ShotResult(HitType.SunkShip, hitShip);
    }
}