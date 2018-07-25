using System;
using MazeGenerator.GameGenerator;
using MazeGenerator.Logic;
using MazeGenerator.Models;
using MazeGenerator.TeleBot;
using MazeGenerator.Tools;

namespace MazeGenerator

{
    internal class Program
    {
        public static (int, Direction) MoveDirection()
        {
            do
            {
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        return (0, Direction.North);
                    case ConsoleKey.RightArrow:
                        return (0, Direction.East);
                    case ConsoleKey.DownArrow:
                        return (0, Direction.South);
                    case ConsoleKey.LeftArrow:
                        return (0, Direction.West);
                    case ConsoleKey.X:
                        return (1, Direction.North);
                    case ConsoleKey.Z:
                        return (2, Direction.North);
                }
            } while (true);
        }

        private static void Main(string[] args)
        {
            //var token = Console.ReadLine();
            //var bot = new MazeBot(token);
            var lobby = new Lobby(1);
            LobbyGenerator.GenerateLobbyMaze(lobby);

            var player1 = new Player
            {
                UserCoordinate = lobby.Maze.GenerateRandomPosition(),
                Rotate = Direction.North,
                PlayerId = 1
            };
            var player2 = new Player
            {
                UserCoordinate = lobby.Maze.GenerateRandomPosition(),
                Rotate = Direction.North,
                PlayerId = 2
            };
            var player3 = new Player
            {
                UserCoordinate = lobby.Maze.GenerateRandomPosition(),
                Rotate = Direction.North,
                PlayerId = 3
            };
            var player4 = new Player
            {
                UserCoordinate = lobby.Maze.GenerateRandomPosition(),
                Rotate = Direction.North,
                PlayerId = 4
            };

            lobby.Players.Add(player1);
            lobby.Players.Add(player2);
            lobby.Players.Add(player3);
            lobby.Players.Add(player4);

            var stroke = 0;
            while (true)
            {
                FormatAnswers.ConsoleApp(lobby);
                var act = MoveDirection();
                if (act.Item1 == 1)
                    MazeLogic.TryShoot(lobby, lobby.Players[stroke], MoveDirection().Item2);
                if (act.Item1 == 2)
                {
                    MazeLogic.Bomb(lobby, lobby.Players[stroke], MoveDirection().Item2);
                }
                else
                {
                    var res = MazeLogic.TryMove(lobby, lobby.Players[stroke], act.Item2);
                    if (res == MazeObjectType.Exit)
                    {
                        if (lobby.Players[stroke].Chest?.IsReal == false)
                        {
                            var r = lobby.Chests.Find(e =>
                                Equals(lobby.Players[stroke].UserCoordinate, e.Position));
                            lobby.Chests.Remove(r);
                            lobby.Players[stroke].Chest = null;
                        }
                        else
                        {
                            //TODO: end game here
                        }
                    }
                }

                stroke++;
                if (stroke == lobby.Players.Count)
                    stroke = 0;
                Console.Clear();
            }

            Console.WriteLine("игра закончена");
            Console.ReadLine();
            //bot.BotClient.StopReceiving();
        }
    }
}