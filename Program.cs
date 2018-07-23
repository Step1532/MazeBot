using System;
using MazeGenerator.GameGenerator;
using MazeGenerator.MazeLogic;
using MazeGenerator.Models;
using MazeGenerator.TeleBot;
using MazeGenerator.Tools;
using Telegram.Bot.Requests;

namespace MazeGenerator

{
    internal class Program
    {
        private static void Main(string[] args)
        {
           //var token = Console.ReadLine();
           //var bot = new MazeBot(token);
            Lobby lobby = new Lobby(1);
            LobbyGenerator.GenerateLobby(lobby);
            Player player1 = new Player();
            Player player2 = new Player();
            Player player3 = new Player();
            Player player4 = new Player();
            player1.UserCoordinate = lobby.Maze.GenerateRandomPosition();
            player1.Rotate = Direction.North;
            player1.PlayerId = 1;
            lobby.Players.Add(player1);
            player2.UserCoordinate = lobby.Maze.GenerateRandomPosition();
            player2.Rotate = Direction.North;
            player2.PlayerId = 2;
            lobby.Players.Add(player2);
            player3.UserCoordinate = lobby.Maze.GenerateRandomPosition();
            player3.Rotate = Direction.North;
            player3.PlayerId = 3;
            lobby.Players.Add(player3);
            player4.UserCoordinate = lobby.Maze.GenerateRandomPosition();
            player4.Rotate = Direction.North;
            player4.PlayerId = 4;
            lobby.Players.Add(player4);
            int stroke = 0;
                while (true)
                {
                    FormatAnswers.ConsoleApp(lobby);
                    var act = MoveDirection();
                    if (act.Item1 == true)
                        Logic.MazeLogic.Shoot(lobby, lobby.Players[stroke], MoveDirection().Item2);
                    else
                        Logic.MazeLogic.TryMove(lobby, lobby.Players[stroke], act.Item2);
                    stroke++;
                    if (stroke == lobby.Players.Count)
                        stroke = 0;
                    Console.Clear();
                }

            Console.ReadLine();
           //bot.BotClient.StopReceiving();
        }

        public static (bool, Direction) MoveDirection()
        {
            do
            {
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        return (false, Direction.North);
                    case ConsoleKey.RightArrow:
                        return (false, Direction.East);
                    case ConsoleKey.DownArrow:
                        return (false, Direction.South);
                    case ConsoleKey.LeftArrow:
                        return (false, Direction.West);
                    case ConsoleKey.X:
                        return (true, Direction.North);
                }
            } while (true);
        }
    }
}