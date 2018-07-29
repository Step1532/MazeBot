using System;
using System.Collections.Generic;
using System.Linq;
using MazeGenerator.Models;
using MazeGenerator.Models.Enums;

namespace MazeGenerator.Core.Services
{
    public static class LobbyService
    {
        /// <summary>
        ///     Проверка что находится в клетке
        /// </summary>
        public static List<MazeObjectType> CheckLobbyCoordinate(Coordinate coord, Lobby lobby)
        {
            var events = new List<MazeObjectType>();

            if (coord.X < 0 || coord.Y < 0 || coord.X >= lobby.Maze.GetLength(1) || coord.Y >= lobby.Maze.GetLength(0))
            {
                events.Add(MazeObjectType.Space);
                return events;
            }


            if (lobby.Events.Any(e => Equals(e.Position, coord)))
                events.Add(MazeObjectType.Event);

            if (lobby.Players.Any(e => Equals(e.UserCoordinate, coord)))
                events.Add(MazeObjectType.Player);

            if (lobby.Maze[coord.X, coord.Y] == 1)
                events.Add(MazeObjectType.Wall);

            if ((coord.X == 0 || coord.Y == 0 || coord.X == lobby.Maze.GetLength(1) - 1 ||
                 coord.Y == lobby.Maze.GetLength(0) - 1) && lobby.Maze[coord.X, coord.Y] == 0)
                events.Add(MazeObjectType.Exit);

            if (lobby.Maze[coord.X, coord.Y] == 0)
                events.Add(MazeObjectType.Void);

            return events;
        }

        /// <summary>
        ///     Проверка что находится в клетке
        /// </summary>
        /// //TODO rename EventsOnTale
        public static List<EventTypeEnum> WhatsEvent(Coordinate coord, Lobby lobby)
        {
            return lobby.Events.Where(e => Equals(e.Position, coord)).Select(e => e.Type).ToList();
        }

        /// <summary>
        ///     Проверка что за клад
        /// </summary>
        public static Treasure PickChest(Coordinate coord, Lobby lobby,  Player player)
        {
            var res = lobby.Chests.Find(e => Equals(e.Position, coord));
            res.Position = player.UserCoordinate;
            var tr = lobby.Events.Find(e => Equals(player.UserCoordinate, e.Position));
            lobby.Events.Remove(tr);
            return res;
        }
    }
}