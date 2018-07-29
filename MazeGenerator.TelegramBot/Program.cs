using System;
using MazeGenerator.Core.GameGenerator;
using MazeGenerator.Core.Services;
using MazeGenerator.Core.Tools;
using MazeGenerator.Models;
using MazeGenerator.Models.Enums;

namespace MazeGenerator.TelegramBot
{
    class Program
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
            var token = Console.ReadLine();
            var bot = new MazeBot(token);
            var lobby = new Lobby(1);
            LobbyGenerator.GenerateLobbyMaze(lobby);

            var player1 = new Player
            {
                UserCoordinate = lobby.Maze.GenerateRandomPosition(),
                Rotate = Direction.North,
                Health = 3,
                PlayerId = 1
            };
            var player2 = new Player
            {
                UserCoordinate = lobby.Maze.GenerateRandomPosition(),
                Rotate = Direction.North,
                Health = 3,
                PlayerId = 2
            };
            var player3 = new Player
            {
                UserCoordinate = lobby.Maze.GenerateRandomPosition(),
                Rotate = Direction.North,
                Health = 3,
                PlayerId = 3
            };
            var player4 = new Player
            {
                UserCoordinate = lobby.Maze.GenerateRandomPosition(),
                Rotate = Direction.North,
                Health = 3,
                PlayerId = 4
            };

            lobby.Players.Add(player1);
            lobby.Players.Add(player2);
            lobby.Players.Add(player3);
            lobby.Players.Add(player4);

            var stroke = 0;
            string Answer = " ";
            while (true)
            {
                FormatAnswers.ConsoleApp(lobby);
                FormatAnswers.PlayerStat(lobby);
                Console.WriteLine(Answer);
                var act = MoveDirection();
                if (act.Item1 == 1)
                {
                    if (MazeLogic.TryShoot(lobby.Players[stroke]))
                    {
                        var res = MazeLogic.Shoot(lobby, lobby.Players[stroke], MoveDirection().Item2);
                        if (res.Item2 != null)
                        {
                            //TODO: генерирование месежа
                        }
                        else
                        {
                            //Todo: генерация месежа
                        }
                    }
                }  
                else if (act.Item1 == 2)
                {
                       var res =  MazeLogic.Bomb(lobby, lobby.Players[stroke], MoveDirection().Item2);
                    if (res == ResultBomb.Wall)
                    {
                        //Todo генерация месежа
                    }
                    else if (res == ResultBomb.NoBomb)
                    {
                        //Todo: генерация месежа
                    }
                    else
                    {
                        //TOdo: генерация месежа
                    }
                }
                else
                {
                    var res = MazeLogic.TryMove(lobby, lobby.Players[stroke], act.Item2);
                    if (res.Contains(PlayerAction.GameEnd))
                    {
                        break;
                    }
                }

                stroke++;
                if (stroke == lobby.Players.Count)
                    stroke = 0;
                Console.Clear();
            }

            Console.WriteLine($"игра закончена, winner {lobby.Players[stroke].PlayerId}");
            Console.ReadLine();
            bot.BotClient.StopReceiving();
        }
    }
}
