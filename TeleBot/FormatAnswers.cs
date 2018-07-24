using System;
using MazeGenerator.Logic;
using MazeGenerator.MazeLogic;
using MazeGenerator.Models;

namespace MazeGenerator.TeleBot
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
                    if (LobbyService.CheckLobbyCoordinate(new Coordinate(j, i), lobby) == MazeObjectType.Event)
                        Console.Write(LobbyService.WhatsEvent(new Coordinate(j, i), lobby));
                    else
                    {
                        var p = lobby.Players.Find(e => Equals(e.UserCoordinate, new Coordinate(j, i)));
                        if (p != null)
                            Console.Write("p" + p.PlayerId);
                        else
                            Console.Write(lobby.Maze[j, i] == 0 ? "  " : "0 ");
                    }

                }
                Console.WriteLine();
            }
            Console.WriteLine(lobby.Events[1].Position.X + " " + lobby.Events[1].Position.Y);
        }
    } 
}