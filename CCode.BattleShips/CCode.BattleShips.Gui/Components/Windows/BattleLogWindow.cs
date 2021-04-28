using System;
using System.Collections.Generic;
using CCode.BattleShips.Core.Events;
using CCode.BattleShips.Core.Services;
using CCode.BattleShips.Gui.Constants;
using Terminal.Gui;

namespace CCode.BattleShips.Gui.Components.Windows
{
    public class BattleLogWindow : IDisposable
    {
        public readonly Window Window;
        private readonly IGameService _gameService;
        private Dictionary<int, Label> _labels;

        public BattleLogWindow(int x, int y, IGameService gameService)
        {
            _gameService = gameService;
            Window = new Window(new Rect(x, y, 118, 14), "Battle Log");
            BuildLabels();
            gameService.PlacedShot += gameServiceOnPlacedShot;
            gameService.GameEnded += gameServiceOnGameEnded;
        }

        public void Dispose()
        {
            _gameService.PlacedShot -= gameServiceOnPlacedShot;
            _gameService.GameEnded -= gameServiceOnGameEnded;
            foreach (var keyValuePair in _labels)
            {
                keyValuePair.Value.Dispose();
            }
            Window?.Dispose();
        }

        private void BuildLabels()
        {
            _labels = new Dictionary<int, Label>();
            var j = 11;
            for (var i = 1; i <= 12; i++)
            {
                var label = new Label(new Rect(0, j, 116, 1), "");
                _labels.Add(i, label);
                Window.Add(label);
                j--;
            }
            _labels[1].ColorScheme = Colors.TopLevel;
        }

        private void gameServiceOnPlacedShot(PlacedShotEventArgs args)
        {
            ShiftAllMessagesUp();
            SetLastMessage(
                $"{GuiTexts.PlayerTypeDictionary[args.Player]} shoot at {args.ShotResult.Coordinate}. {GuiTexts.HitTypeDictionary[args.ShotResult.HitType]} {args.ShotResult.HitShip?.Name}");
        }

        private void gameServiceOnGameEnded(GameStateChangedEventArgs args)
        {
            ShiftAllMessagesUp();
            SetLastMessage($"{GuiTexts.PlayerTypeDictionary[args.Player]} won");
        }

        private void ShiftAllMessagesUp()
        {
            for (var i = 11; i >= 1; i--)
            {
                _labels[i + 1].Text = _labels[i].Text;
            }
        }

        private void SetLastMessage(string message) => _labels[1].Text = message;
    }
}