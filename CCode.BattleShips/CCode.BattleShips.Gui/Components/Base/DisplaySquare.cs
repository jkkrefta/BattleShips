using System;
using CCode.BattleShips.Core.Enums;
using CCode.BattleShips.Core.Events;
using CCode.BattleShips.Core.Models;
using CCode.BattleShips.Core.Services;
using Terminal.Gui;

namespace CCode.BattleShips.Gui.Components.Base
{
    public class DisplaySquare : IDisposable
    {
        public Button Button { get; }
        private readonly Coordinate _coordinate;
        private readonly IGameService _gameService;

        public DisplaySquare(int x, int y, Coordinate coordinate, IGameService gameService)
        {
            _coordinate = coordinate;
            _gameService = gameService;
            Button = new Button(x, y, " ") {CanFocus = false};
            if (gameService.CoordinateContainsShip(PlayerType.HumanPlayer, _coordinate))
            {
                Button.Text = " ";
                Button.ColorScheme = Colors.Error;
            }
            gameService.PlacedShot += gameServiceOnPlacedShot;
        }

        public void Dispose()
        {
            _gameService.PlacedShot -= gameServiceOnPlacedShot;
            Button?.Dispose();
        }

        private void gameServiceOnPlacedShot(PlacedShotEventArgs args)
        {
            if (args.Player != PlayerType.ComputerPlayer ||
                args.ShotResult.Coordinate != _coordinate) return;
                
            switch (args.ShotResult.HitType)
            {
                case HitType.Miss:
                    Button.ColorScheme = Colors.Menu;
                    Button.Text = "·";
                    break;
                case HitType.Hit:
                case HitType.SunkShip:
                    Button.ColorScheme = Colors.TopLevel;
                    Button.Text = "X";
                    break;
            }
        }
    }
}