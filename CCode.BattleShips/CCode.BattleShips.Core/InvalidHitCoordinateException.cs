using System;

namespace CCode.BattleShips.Core
{
    public class InvalidHitCoordinateException : Exception
    {
        public InvalidHitCoordinateException(string message) : base(message)
        {
        }
    }
}