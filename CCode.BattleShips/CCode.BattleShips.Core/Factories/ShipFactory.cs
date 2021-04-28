using System;
using System.Collections.Generic;
using System.Linq;
using CCode.BattleShips.Core.DTO;
using CCode.BattleShips.Core.Exceptions;
using CCode.BattleShips.Core.Models;
using CCode.BattleShips.Core.Validators;

namespace CCode.BattleShips.Core.Factories
{
    public interface IShipFactory
    {
        Ship Create(ShipDefinition definition, List<Coordinate> coordinates);
    }

    public class ShipFactory : IShipFactory
    {
        private readonly IShipLayoutValidator _layoutValidator;
        
        public ShipFactory(IShipLayoutValidator layoutValidator)
        {
            _layoutValidator = layoutValidator;
        }
        
        public Ship Create(ShipDefinition definition, List<Coordinate> coordinates)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }
            
            if (coordinates == null)
            {
                throw new ArgumentNullException(nameof(coordinates));
            }
            
            if (!coordinates.Any())
            {
                throw new InvalidCoordinateException("Coordinates cannot empty");
            }
            
            _layoutValidator.ValidateShipSize(coordinates, definition.Size);
            _layoutValidator.ValidateShipLayout(coordinates);
            return new Ship(definition.Name, coordinates);
        }
    }
}