using System;
using System.Collections.Generic;
using System.Linq;
using CCode.BattleShips.Core.Models;
using CCode.BattleShips.Core.Services;
using CCode.BattleShips.Gui.Components.Base;
using CCode.BattleShips.Gui.Extensions;
using CCode.BattleShips.Gui.Utils;
using Terminal.Gui;

namespace CCode.BattleShips.Gui.Components.Windows
{
    public class ShootingWaterBoardWindow : IDisposable
    {
        public readonly Window Window;
        private readonly IGameService _gameService;
        private readonly List<ShotableSquare> _shotableSquares;

        public ShootingWaterBoardWindow(int x, int y, IGameService gameService)
        {
            _gameService = gameService;
            Window = new Window(new Rect(x, y, 55, 13), "Computer Waters");
            Window.Add(GenerateColumnNumbers());
            Window.Add(GenerateRowLetters());
            _shotableSquares = GenerateShootingSquares();
            Window.Add(_shotableSquares.Select(square => square.Button));
        }

        public void Dispose()
        {
            _shotableSquares?.ForEach(x => x.Dispose());
            Window?.Dispose();
        }

        private List<ShotableSquare> GenerateShootingSquares() => GuiUtils.Generate2DCollection(10, 10, (x, y) => new ShotableSquare(x * 5 + 3, y + 1, new Coordinate(x+1, y+1), _gameService));

        private static List<Label> GenerateRowLetters() => GuiUtils.GenerateCollection(10, y => new Label(0, y + 1, $"{(char)('A'+y)}"));

        private static List<Label> GenerateColumnNumbers() => GuiUtils.GenerateCollection(10, x => new Label(x * 5 + 5, 0, $"{x + 1}"));
    }
}