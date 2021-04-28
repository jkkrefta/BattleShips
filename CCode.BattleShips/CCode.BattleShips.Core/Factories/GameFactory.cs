using System.Collections.Generic;
using CCode.BattleShips.Core.Constants;
using CCode.BattleShips.Core.DTO;
using CCode.BattleShips.Core.Enums;
using CCode.BattleShips.Core.Models;

namespace CCode.BattleShips.Core.Factories
{
    public interface IGameFactory
    {
        Game Create();
    }

    public class GameFactory : IGameFactory
    {
        private readonly IPopulatedWatersFactory _populatedWatersFactory;

        public GameFactory(IPopulatedWatersFactory populatedWatersFactory)
        {
            _populatedWatersFactory = populatedWatersFactory;
        }

        public Game Create()
        {
            var game = new Game
            {
                CurrentState = new GameState(GamePhase.Initializing, PlayerType.None)
            };
            var shipDefinitions = new List<ShipDefinition>
            {
                ShipDefinitions.Battleship,
                ShipDefinitions.Destroyer,
                ShipDefinitions.Destroyer
            };
            game.ComputerWaters = _populatedWatersFactory.CreateWaterWithRandomlyPlacedShips(shipDefinitions);
            game.HumanWaters = _populatedWatersFactory.CreateWaterWithRandomlyPlacedShips(shipDefinitions);
            return game;
        }
    }
}