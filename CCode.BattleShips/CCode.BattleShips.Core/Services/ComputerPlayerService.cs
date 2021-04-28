using System;
using System.Collections.Generic;
using CCode.BattleShips.Core.Enums;
using CCode.BattleShips.Core.Events;
using CCode.BattleShips.Core.Exceptions;
using CCode.BattleShips.Core.Models;

namespace CCode.BattleShips.Core.Services
{
    public class ComputerPlayerService
    {
        private readonly IGameService _gameService;
        private readonly Random _random;
        private readonly List<Coordinate> _shotsAlreadyPlaced = new();

        public ComputerPlayerService(IGameService gameService, Random random)
        {
            _gameService = gameService;
            _random = random;
            _gameService.GameStateChanged += gameServiceOnGameStateChanged();
        }

        private Action<GameStateChangedEventArgs> gameServiceOnGameStateChanged()
        {
            return args =>
            {
                if (args.Phase != GamePhase.PlayersAction || args.Player != PlayerType.ComputerPlayer) return;
                PlaceRandomShot();
            };
        }

        private void PlaceRandomShot()
        {
            var isPlacingShot = true;
            while (isPlacingShot)
            {
                try
                {
                    var shot = new Coordinate(_random.Next(1, 11), _random.Next(1, 11));
                    if (_shotsAlreadyPlaced.Contains(shot)) continue;
                    if (!_gameService.Game.HumanWaters.CanPlaceShot(shot)) continue;
                    _gameService.PlaceShot(shot);
                    _shotsAlreadyPlaced.Add(shot);
                    isPlacingShot = false;
                }
                catch (AlreadyHitCoordinateException e)
                {
                }
            }
        }
    }
}