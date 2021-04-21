using System;

namespace CCode.BattleShips.Core.Exceptions
{
    public class InvalidGameStateException : Exception
    {
        public InvalidGameStateException(string message) : base(message)
        {
        }
    }
}