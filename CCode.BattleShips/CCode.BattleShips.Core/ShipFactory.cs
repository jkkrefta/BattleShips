using System;
using System.Collections.Generic;
using System.Linq;
using CCode.BattleShips.Core.Exceptions;

namespace CCode.BattleShips.Core
{
    public class ShipFactory
    {
        private readonly IShipLayoutValidator _layoutValidator;
        
        public ShipFactory(IShipLayoutValidator layoutValidator)
        {
            _layoutValidator = layoutValidator;
        }
        
        public Ship Create(ShipType type, List<Coordinate> coordinates)
        {
            if (coordinates == null)
            {
                throw new ArgumentNullException(nameof(coordinates));
            }
            
            if (!coordinates.Any())
            {
                throw new InvalidCoordinateException("Coordinates cannot empty");
            }

            switch (type)
            {
                case ShipType.Destroyer:
                    _layoutValidator.ValidateShipSize(coordinates, 4);
                    _layoutValidator.ValidateShipLayout(coordinates);
                    return new Ship(ShipType.Destroyer, coordinates);
                case ShipType.Battleship:
                    _layoutValidator.ValidateShipSize(coordinates, 5);
                    _layoutValidator.ValidateShipLayout(coordinates);
                    return new Ship(ShipType.Battleship, coordinates);
                default:
                    throw new InvalidShipTypeException("Cannot create Undefined type ship");
            }
        }
    }
}