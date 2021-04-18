using System;

namespace CCode.BattleShips.Core.Exceptions
{
    public class InvalidHitCoordinateException : Exception
    {
        public InvalidHitCoordinateException(string message) : base(message)
        {
        }
    }
}