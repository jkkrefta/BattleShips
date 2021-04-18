using System;

namespace CCode.BattleShips.Core.Exceptions
{
    public class InvalidShipTypeException : Exception
    {
        public InvalidShipTypeException(string message) : base(message)
        {
        }
    }
}