using System;
using System.Collections.Generic;
using System.Linq;
using CCode.BattleShips.Core.Models;

namespace CCode.BattleShips.Core.Generators
{
    public interface ICoordinateFactory
    {
        IEnumerable<Coordinate> CreateRandomLine(int size);
    }

    public class CoordinateFactory : ICoordinateFactory
    {
        private readonly Random _random;

        public CoordinateFactory(Random random)
        {
            _random = random;
        }
        
        public IEnumerable<Coordinate> CreateRandomLine(int size)
        {
            var isHorizontal = _random.Next(0, 1000) / 500;
            return isHorizontal == 1
                ? CreateRandomHorizontalLine(size) 
                : CreateRandomVerticalLine(size);
        }
        
        public IEnumerable<Coordinate> CreateRandomHorizontalLine(int size)
        {
            var y = _random.Next(1, 11);
            return GenerateRange(size).Select(x => new Coordinate(x, y));
        }

        public IEnumerable<Coordinate> CreateRandomVerticalLine(int size)
        {
            var x = _random.Next(1, 11);
            return GenerateRange(size).Select(y => new Coordinate(x, y));
        }
        
        private IEnumerable<int> GenerateRange(int size)
        {
            return Enumerable.Range(_random.Next(1, 11 - size + 1), size);
        }
    }
}