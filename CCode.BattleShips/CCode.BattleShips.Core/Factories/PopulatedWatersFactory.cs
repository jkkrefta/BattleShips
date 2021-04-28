using System.Collections.Generic;
using System.Linq;
using CCode.BattleShips.Core.DTO;
using CCode.BattleShips.Core.Exceptions;
using CCode.BattleShips.Core.Generators;
using CCode.BattleShips.Core.Models;

namespace CCode.BattleShips.Core.Factories
{
    public interface IPopulatedWatersFactory
    {
        Waters CreateWaterWithRandomlyPlacedShips(List<ShipDefinition> shipList);
    }

    public class PopulatedWatersFactory : IPopulatedWatersFactory
    {
        private readonly IShipFactory _shipFactory;
        private readonly ICoordinateFactory _coordinateFactory;

        public PopulatedWatersFactory(IShipFactory shipFactory, ICoordinateFactory coordinateFactory)
        {
            _coordinateFactory = coordinateFactory;
            _shipFactory = shipFactory;
        }
        
        public Waters CreateWaterWithRandomlyPlacedShips(List<ShipDefinition> shipList)
        {
            var waters = new Waters();

            var i = 0;
            while (i < shipList.Count)
            {
                try
                {
                    var coordinates = _coordinateFactory.CreateRandomLine(shipList[i].Size).ToList();
                    var ship = _shipFactory.Create(shipList[i], coordinates);
                    if (!waters.CanPlaceShip(ship)) continue;
                    waters.PlaceShip(ship);
                    i++;
                }
                catch (InvalidShipPlacementException e)
                {
                    // retry if occurs
                }
            }

            return waters;
        }
    }
}