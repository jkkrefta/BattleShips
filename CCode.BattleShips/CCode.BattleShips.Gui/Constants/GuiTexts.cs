using System.Collections.Generic;
using CCode.BattleShips.Core.Enums;

namespace CCode.BattleShips.Gui.Constants
{
    public static class GuiTexts
    {
        public static Dictionary<PlayerType, string> PlayerTypeDictionary = new()
        {
            {PlayerType.ComputerPlayer, "Computer"},
            {PlayerType.HumanPlayer, "Player"}
        };

        public static Dictionary<HitType, string> HitTypeDictionary = new()
        {
            {HitType.Miss, "Miss"},
            {HitType.Hit, "Hit"},
            {HitType.SunkShip, "Sunk"}
        };

        public static Dictionary<GamePhase, string> GameStatusDictionary = new()
        {
            {GamePhase.PlayersAction, "Waiting for action of"},
            {GamePhase.Victory, "Victory of"}
        };
    }
}