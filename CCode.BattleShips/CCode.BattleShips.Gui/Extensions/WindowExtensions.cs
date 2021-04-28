using System.Collections.Generic;
using Terminal.Gui;

namespace CCode.BattleShips.Gui.Extensions
{
    public static class WindowExtensions
    {
        public static void Add(this Window window, IEnumerable<View> views)
        { 
            foreach (var view in views)
            {
                window.Add(view);
            }
        }
    }
}