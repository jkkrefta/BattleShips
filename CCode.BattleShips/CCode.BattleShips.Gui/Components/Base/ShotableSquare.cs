using System;
using CCode.BattleShips.Core.Enums;
using CCode.BattleShips.Core.Events;
using CCode.BattleShips.Core.Models;
using CCode.BattleShips.Core.Services;
using Terminal.Gui;

namespace CCode.BattleShips.Gui.Components.Base
{
    public class ShotableSquare : IDisposable
    {
        public Button Button { get; }
        private readonly Coordinate _coordinate;
        private readonly IGameService _gameService;
        private bool _wasHit;
        private bool _buttonEventsAttached;

        public ShotableSquare(int x, int y, Coordinate coordinate, IGameService gameService)
        {
            _gameService = gameService;
            _coordinate = coordinate;
            Button = new Button(x, y, " ") {CanFocus = false};
            Button.MouseEnter += buttonOnMouseEnter;
            Button.MouseLeave += buttonOnMouseLeave;
            Button.MouseClick += buttonOnMouseClick;
            _buttonEventsAttached = true;
            _gameService.GameEnded += gameServiceOnGameEnded;
        }

        public void Dispose()
        {
            if (_buttonEventsAttached)
            {
                Button.MouseEnter -= buttonOnMouseEnter;
                Button.MouseLeave -= buttonOnMouseLeave;
                Button.MouseClick -= buttonOnMouseClick;
            }
            _gameService.GameEnded -= gameServiceOnGameEnded;
            Button?.Dispose();
        }

        private void gameServiceOnGameEnded(GameStateChangedEventArgs _)
        {
            Button.MouseEnter -= buttonOnMouseEnter;
            Button.MouseLeave -= buttonOnMouseLeave;
            Button.MouseClick -= buttonOnMouseClick;
            _buttonEventsAttached = false;
        }

        private void buttonOnMouseLeave(View.MouseEventArgs _)
        {
            if (_wasHit || _gameService.CurrentPlayer != PlayerType.HumanPlayer) return;
            Button.ColorScheme = Colors.Base;
            Button.Text = Button.Text;
        }

        private void buttonOnMouseEnter(View.MouseEventArgs _)
        {
            if (_wasHit || _gameService.CurrentPlayer != PlayerType.HumanPlayer) return;
            Button.ColorScheme = Colors.Dialog;
            Button.Text = Button.Text;
        }

        private void buttonOnMouseClick(View.MouseEventArgs _)
        {
            if (_wasHit || _gameService.CurrentPlayer != PlayerType.HumanPlayer) return;
            var result = _gameService.PlaceShot(_coordinate);
            _wasHit = true;
            switch (result.HitType)
            {
                case HitType.Miss:
                    Button.ColorScheme = Colors.Menu;
                    Button.Text = "·";
                    break;
                case HitType.Hit or HitType.SunkShip:
                    Button.ColorScheme = Colors.TopLevel;
                    Button.Text = "X";
                    break;
            }
        }
    }
}