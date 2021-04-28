using CCode.BattleShips.Core.DTO;

namespace CCode.BattleShips.Core.Constants
{
    public static class ShipDefinitions
    {
        public static ShipDefinition Battleship = new() {Name = nameof(Battleship), Size = 5};
        public static ShipDefinition Destroyer = new() {Name = nameof(Destroyer), Size = 4};
    }
}