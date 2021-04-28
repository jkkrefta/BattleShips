using System;
using System.Collections.Generic;

namespace CCode.BattleShips.Gui.Utils
{
    public static class GuiUtils
    {
        public static List<T> GenerateCollection<T>(int numberOfElements, Func<int, T> creationFunction)
        {
            var collection = new List<T>();
            for (var i = 0; i < numberOfElements; i++)
            {
                collection.Add(creationFunction.Invoke(i));
            }
            return collection;
        }
        
        public static List<T> Generate2DCollection<T>(int numberOfCols, int numberOfRows, Func<int, int, T> creationFunction)
        {
            var collection = new List<T>();
            for (var i = 0; i < numberOfCols; i++)
            {
                for (var j = 0; j < numberOfRows; j++)
                {
                    collection.Add(creationFunction.Invoke(i, j));
                }
            }
            return collection;
        }
    }
}