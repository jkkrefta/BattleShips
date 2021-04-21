using System.Collections.Generic;
using System.Linq;
using CCode.BattleShips.Core.DTO;
using CCode.BattleShips.Core.Enums;
using CCode.BattleShips.Core.Factories;
using CCode.BattleShips.Core.Models;

namespace CCode.BattleShips.Core
{
    public class GameService
    {
        public Game Game { get; private set; }
        private readonly ShipFactory _shipFactory;

        public GameService(ShipFactory shipFactory)
        {
            _shipFactory = shipFactory;
        }


        public void InitializeNewGame()
        {
            Game = new Game();
            Game.Initialize();
            Game.EnqueuePlacingShipsForPlayer(PlayerType.HumanPlayer);
        }

        public void CreateShipOnWaters(ShipType shipType, List<Coordinate> shipCoordinates)
        {
            var playersWaters = Game.GetCurrentPlayersWaters();
            var newShip = _shipFactory.Create(shipType, shipCoordinates);
            playersWaters.PlaceShip(newShip);
        }
        
        public void PlayerPlacedAllShips()
        {
            Game.EnqueuePlacingShipsForNextPlayer();
        }

        public void BothPlayersPlacedAllShips()
        {
            Game.EnqueueCurrentPlayersAction();
        }
        
        public ShotResult PlaceShootOnWaters(Coordinate shot)
        {
            Game.EnqueueEvaluatingCurrentPlayersAction();
            var targetWaters = Game.GetOtherPlayersWaters();
            return targetWaters.PlaceShot(shot);
        }
        
        public void EvaluateShotResult(ShotResult shotResult)
        {
            switch (shotResult.HitType)
            {
                case HitType.Miss:
                    Game.EnqueueNextPlayersAction();
                    break;
                case HitType.Hit:
                    Game.EnqueueCurrentPlayersAction();
                    break;
                case HitType.SunkShip:
                    Game.EnqueueCheckingVictoryConditionsCurrentPlayersAction();
                    break;
            }
        }
        
        public void EvaluateVictoryConditions()
        {
            var otherPlayersWaters = Game.GetOtherPlayersWaters();
            if (otherPlayersWaters.Ships.Any(ship => !ship.IsSunk))
            {
                Game.EnqueueVictory();
            }
            Game.EnqueueCurrentPlayersAction();
        }
    }
}