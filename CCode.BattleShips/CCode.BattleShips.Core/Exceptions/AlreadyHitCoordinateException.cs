using System;

namespace CCode.BattleShips.Core.Exceptions
{
    public class AlreadyHitCoordinateException : Exception
    {
        public AlreadyHitCoordinateException(string message) : base(message)
        {
        }
    }
}