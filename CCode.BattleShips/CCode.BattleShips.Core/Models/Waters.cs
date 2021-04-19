using System.Collections.Generic;

namespace CCode.BattleShips.Core.Models
{
    public class Waters
    {
        public List<Ship> Ships { get; } = new();
        public List<Coordinate> MissedShots { get; } = new();
    }
}