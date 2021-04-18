using System;

namespace CCode.BattleShips.Core.Exceptions
{
    public class InvalidCoordinateException : Exception
    {
        public InvalidCoordinateException(string message) : base(message)
        {
        }
    }
}