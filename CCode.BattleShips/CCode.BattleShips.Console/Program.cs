using System;
using System.Collections.Generic;
using Terminal.Gui;

namespace CCode.BattleShips.Console
{
    class Program
    {
        static void Main()
        {
            Application.Init ();
            
            var menu = new MenuBar(new MenuBarItem[]
            {
                new MenuBarItem("_BattleShips", new MenuItem[]
                {
                    new MenuItem("_NewGame", "", () => { Application.RequestStop(); }),
                    new MenuItem("_Quit", "", () => { Application.RequestStop(); })
                }),
            });
            
            var win = new Window("")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill() - 1
            };

            CreateWaterBoard(win, 2, 2, "Home Zone");
            CreateWaterBoard(win, 60, 2, "Enemy Zone");
            
            Application.Top.Add (menu, win);
            Application.Run ();
        }

        private static void CreateWaterBoard(View view, int x, int y, string zoneName)
        {
            view.Add(new Label(x, y, zoneName));
            GenerateCollection(10, yy => new Label(0 + x, yy * 2 + 2 + y, "─────────────────────────────────────────────────────")).ForEach(view.Add);
            GenerateCollection(10, xx => new Label(xx * 5 + 5 + x, 1 + y, $"{xx+1}")).ForEach(view.Add);
            GenerateCollection(10, yy => new Label(1 + x, yy * 2 + 3 + y, $"{(char)('A'+yy)}")).ForEach(view.Add);
            Generate2DCollection(10, 10, (xx, yy) => new Button(xx * 5 + 3 + x, yy * 2 + 3 + y, " ")).ForEach(view.Add);
        }
        
        private static List<T> GenerateCollection<T>(int numberOfElements, Func<int, T> creationFunction)
        {
            var collection = new List<T>();
            for (var i = 0; i < numberOfElements; i++)
            {
                collection.Add(creationFunction.Invoke(i));
            }
            return collection;
        }
        
        private static List<T> Generate2DCollection<T>(int numberOfCols, int numberOfRows, Func<int, int, T> creationFunction)
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