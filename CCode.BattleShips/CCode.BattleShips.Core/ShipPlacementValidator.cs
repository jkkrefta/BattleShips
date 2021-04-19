﻿using CCode.BattleShips.Core.Exceptions;

namespace CCode.BattleShips.Core
{
    public class ShipPlacementValidator
    {
        public void ValidateShipPlacement(Waters waters, Ship ship)
        {
            ship.Coordinates.ForEach(coordinate =>
            {
                waters.Ships.ForEach(shipOnWaters => { ValidateThatShipsDoNotCollide(ship, shipOnWaters, coordinate); });
            });
        }

        private static void ValidateThatShipsDoNotCollide(Ship ship, Ship shipOnWaters, Coordinate coordinate)
        {
            if (shipOnWaters.IsWithinShipCoordinates(coordinate))
            {
                throw new InvalidShipPlacementException(
                    $"Cannot place ship on {string.Join(',', ship.Coordinates)}. There is collision on {coordinate}");
            }
        }
    }
}