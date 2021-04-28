using System;

namespace CCode.BattleShips.Core.Exceptions
{
    public class InvalidShipConstructionException : Exception
    {
        public InvalidShipConstructionException(string message) : base(message)
        {
        }
    }
}