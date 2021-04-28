using System;
using CCode.BattleShips.Core.Factories;
using CCode.BattleShips.Core.Generators;
using CCode.BattleShips.Core.Services;
using CCode.BattleShips.Core.Validators;
using CCode.BattleShips.Gui.Components.Windows;
using Terminal.Gui;

namespace CCode.BattleShips.Console
{
    class Program
    {
        static void Main()
        {
            IGameService gameService = CreateGameService();
            new ComputerPlayerService(gameService, new Random());
            gameService.CreateNewGame();
            Application.Init ();
            Application.Top.Add (CreateMenu(), CreateWindow(gameService));
            gameService.StartGame();
            Application.Run ();
        }

        private static GameService CreateGameService()
        {
            return new(new GameFactory(
                new PopulatedWatersFactory(new ShipFactory(new ShipLayoutValidator()),
                    new CoordinateFactory(new Random()))));
        }

        private static MenuBar CreateMenu()
        {
            return new(new MenuBarItem[]
            {
                new("_BattleShips", new MenuItem[]
                {
                    new("_Quit", "", Application.RequestStop)
                })
            });
        }

        private static Window CreateWindow(IGameService gameService)
        {
            var window = new Window("") {X = 0, Y = 1};
            window.Add(new DisplayOnlyWaterBoardWindow(0, 0, gameService).Window);
            window.Add(new ShootingWaterBoardWindow(62, 0, gameService).Window);
            window.Add(new BattleLogWindow(0, 13, gameService).Window);
            return window;
        }
    }
}