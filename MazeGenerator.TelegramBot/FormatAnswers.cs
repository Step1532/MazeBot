using System;
using System.Linq;
using MazeGenerator.Core.Services;
using MazeGenerator.Models;
using MazeGenerator.Models.Enums;

namespace MazeGenerator.TelegramBot
{
    public static class FormatAnswers
    {
        public static string AnswerUp(bool isAction, string username)
        {
            return null;
        }

        public static void ConsoleApp(Lobby lobby)
        {
            Coordinate a = new Coordinate(0, 0);
            for (int i = 0; i < lobby.Maze.GetLength(1); i++)
            {
                for (int j = 0; j < lobby.Maze.GetLength(0); j++)
                {
                    if (LobbyService.CheckLobbyCoordinate(new Coordinate(j, i), lobby)[0] == MazeObjectType.Event)
                    {
                          Console.Write(EventLetter(LobbyService.EventsOnCell(new Coordinate(j, i), lobby).First()));
                    }
                    else
                    {
                        var p = lobby.Players.Find(e => Equals(e.UserCoordinate, new Coordinate(j, i)));
                        if (p != null)
                            Console.Write("p1");
                        else
                            Console.Write(lobby.Maze[j, i] == 0 ? "  " : "0 ");
                    }

                }
                Console.WriteLine();
            }
   //         Console.WriteLine(lobby.Events[1].Position.X + " " + lobby.Events[1].Position.Y);
        }

        public static string EventLetter(EventTypeEnum type)
        {
            switch (type)
            {
                case EventTypeEnum.Exit:
                    return "E ";
                case EventTypeEnum.Arsenal:
                    return "A ";
                case EventTypeEnum.Hospital:
                    return "+ ";
                case EventTypeEnum.Holes:
                    return "H ";
                case EventTypeEnum.Chest:
                    return "C ";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void PlayerStat(Lobby lobby)
        {
            for (int i = 0; i < lobby.Players.Count; i++)
            {
                Console.Write($"{lobby.Players[i].TelegramUserId} | " +
                              $"{lobby.Players[i].UserCoordinate.X}, {lobby.Players[i].UserCoordinate.Y} | " +
                              $"{lobby.Players[i].Bombs} | {lobby.Players[i].Guns} | {lobby.Players[i].Health} | {lobby.Players[i].Rotate} | ");
                if (lobby.Players[i].Chest == null)
                {
                    Console.WriteLine("null");
                }
                else
                    Console.WriteLine($"{lobby.Players[i].Chest.IsReal}");
            }

            for (int i = 0; i < lobby.Chests.Count; i++)
            {
                Console.WriteLine($"{lobby.Chests[i].Position.X}, {lobby.Chests[i].Position.Y} | {lobby.Chests[i].IsReal}");
            }
        }
    }
}