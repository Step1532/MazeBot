﻿using System;
using System.Diagnostics;
using MazeGenerator.GameGenerator;
using MazeGenerator.Logic;
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
                    if (act.Item1 == 1)
                        Logic.MazeLogic.TryShoot(lobby, lobby.Players[stroke], MoveDirection().Item2);
                    if (act.Item1 == 2)
                        Logic.MazeLogic.Bomb(lobby, lobby.Players[stroke], MoveDirection().Item2);
                    else
                    {
                        var res  = Logic.MazeLogic.TryMove(lobby, lobby.Players[stroke], act.Item2);
                        if (res == MazeObjectType.Exit)
                        {
                            if (lobby.Players[stroke].chest.IsTrue)
                                break;
                            else
                            {
                                var r = lobby.Chests.Find(e =>
                                    Equals(lobby.Players[stroke].UserCoordinate, e.Position));
                                lobby.Chests.Remove(r);
                                lobby.Players[stroke].chest = null;
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

        public static (int, Direction) MoveDirection()
        {
            do
            {
                ConsoleKeyInfo key = Console.ReadKey();
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
    }
}