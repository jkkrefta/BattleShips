using System;

namespace CCode.BattleShips.Core.Exceptions
{
    public class InvalidShipPlacementException : Exception
    {
        public InvalidShipPlacementException(string message) : base(message)
        {
        }
    }
}