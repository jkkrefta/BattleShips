using System.Collections.Generic;
using System.Linq;
using CCode.BattleShips.Core.Exceptions;
using CCode.BattleShips.Core.Models;

namespace CCode.BattleShips.Core.Validators
{
    public interface IShipLayoutValidator
    {
        void ValidateShipLayout(List<Coordinate> coordinates);
        void ValidateShipSize(List<Coordinate> coordinates, int shipSize);
    }

    public class ShipLayoutValidator : IShipLayoutValidator
    {
        public void ValidateShipLayout(List<Coordinate> coordinates)
        {
            var isInHorizontalLine = AreContinousValues(coordinates.Select(x => x.X));
            var isInVerticalLine = AreContinousValues(coordinates.Select(x => x.Y));
            if (!isInHorizontalLine && !isInVerticalLine || isInHorizontalLine && isInVerticalLine)
            {
                throw new InvalidCoordinateException(
                    $"Coordinates must form one line. Given coordinates {string.Join(',', coordinates)}");
            }
        }

        public void ValidateShipSize(List<Coordinate> coordinates, int shipSize)
        {
            if (coordinates.Count != shipSize)
            {
                throw new InvalidCoordinateException($"Ship occupies {shipSize} tiles, {coordinates.Count} ware given");
            }
        }

        private bool AreContinousValues(IEnumerable<int> series)
        {
            var values = series.ToList();
            values.Sort();

            var i = 0;
            while (i + 1 < values.Count)
            {
                if (values[i + 1] - values[i] != 1)
                {
                    return false;
                }

                i++;
            }

            return true;
        }
    }
}