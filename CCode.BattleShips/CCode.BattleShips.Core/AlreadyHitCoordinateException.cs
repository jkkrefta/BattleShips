using System;

namespace CCode.BattleShips.Core
{
    public class AlreadyHitCoordinateException : Exception
    {
        public AlreadyHitCoordinateException(string message) : base(message)
        {
        }
    }
}