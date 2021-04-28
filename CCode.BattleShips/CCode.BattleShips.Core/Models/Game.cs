using System.Linq;
using CCode.BattleShips.Core.DTO;
using CCode.BattleShips.Core.Enums;
using CCode.BattleShips.Core.Exceptions;

namespace CCode.BattleShips.Core.Models
{
    public class Game
    {
        public Waters HumanWaters = new();
        public Waters ComputerWaters = new();
        public GameState CurrentState = new(GamePhase.Initializing, PlayerType.None);

        public ShotResult PlaceShoot(Coordinate target)
        {
            if (CurrentState.Phase != GamePhase.PlayersAction)
            {
                throw new InvalidGameStateException("");
            }
            
            if (CurrentState.Player == PlayerType.ComputerPlayer)
            {
                return HumanWaters.PlaceShot(target);
            }
            
            if (CurrentState.Player == PlayerType.HumanPlayer)
            {
                return ComputerWaters.PlaceShot(target);
            }
            
            throw new InvalidGameStateException("");
        }
        
        public GameState GetNextState(ShotResult shotResult)
        {
            if (CurrentState.Phase == GamePhase.Victory || CurrentState.Phase == GamePhase.Initializing)
                throw new InvalidGameStateException("");

            if (CurrentState.Player == PlayerType.None)
                throw new InvalidGameStateException("");

            switch (shotResult.HitType)
            {
                case HitType.Miss:
                    return new GameState(GamePhase.PlayersAction, GetOtherPlayer());
                case HitType.Hit:
                    return new GameState(GamePhase.PlayersAction, CurrentState.Player);
                case HitType.SunkShip:
                {
                    var otherPlayerWaters = GetOtherPlayerWaters();
                    var anyShipLeft = otherPlayerWaters.Ships.Any(ship => !ship.IsSunk);
                    return anyShipLeft 
                        ? new GameState(GamePhase.PlayersAction, CurrentState.Player) 
                        : new GameState(GamePhase.Victory, CurrentState.Player);
                }
                default:
                    throw new InvalidGameStateException("");
            }
        }

        public bool CoordinateContainsShip(PlayerType player, Coordinate coordinate)
        {
            return GetPlayerWaters(player).Ships.Any(ship => ship.IsWithinShipCoordinates(coordinate));
        }

        private PlayerType GetOtherPlayer()
        {
            return CurrentState.Player switch
            {
                PlayerType.ComputerPlayer => PlayerType.HumanPlayer,
                PlayerType.HumanPlayer => PlayerType.ComputerPlayer,
                _ => throw new InvalidGameStateException("")
            };
        }
        
        private Waters GetOtherPlayerWaters()
        {
            return CurrentState.Player switch
            {
                PlayerType.ComputerPlayer => HumanWaters,
                PlayerType.HumanPlayer => ComputerWaters,
                _ => throw new InvalidGameStateException("")
            };
        }
        
        private Waters GetPlayerWaters(PlayerType player)
        {
            return player switch
            {
                PlayerType.ComputerPlayer => ComputerWaters,
                PlayerType.HumanPlayer => HumanWaters,
                _ => throw new InvalidGameStateException("")
            };
        }
    }
}