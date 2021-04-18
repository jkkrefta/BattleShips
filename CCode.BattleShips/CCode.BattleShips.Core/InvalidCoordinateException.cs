using System;

namespace CCode.BattleShips.Core
{
    public class InvalidCoordinateException : Exception
    {
        public InvalidCoordinateException(string message) : base(message)
        {
        }
    }
}