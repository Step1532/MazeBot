using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGenerator.Models;

namespace MazeGenerator.Logic
{
    public static class LobbyService
    {
        //TODO: вынести из лобби логику лоад и сейв сюда, лобби в модели
        /// <summary>
        /// Проверка что находится в клетке
        /// </summary>
        public static MazeObjectType CheckLobbyCoordinate(Coordinate coord, Lobby lobby)
        {
            if (lobby.Events.Any(e => Equals(e.Position, coord)))
            {
                return MazeObjectType.Event;
            }
            if (lobby.Players.Any(e => Equals(e.UserCoordinate, coord)))
            {
                return MazeObjectType.Player;
            }
            if (lobby.Maze[coord.X, coord.Y] == 1)
                return MazeObjectType.Wall;
            if (coord.X == 0 || coord.Y == 0 || coord.X == lobby.Maze.GetLength(1)-1 || coord.Y == lobby.Maze.GetLength(0)-1)
                return MazeObjectType.Exit;
            if (lobby.Maze[coord.X, coord.Y] == 0)
                return MazeObjectType.Void;

            //TODO: create own exeption
            throw new Exception("CheckCoord");
        }

        //TODO: лучше возвращать EventTypeEnum, а вывод уже в FormatPrint обозначать
        /// <summary>
        /// Проверка что находится в клетке
        /// </summary>
        public static EventTypeEnum WhatsEvent(Coordinate coord, Lobby lobby)
        {
            var res = lobby.Events.Find(e => Equals(e.Position, coord));
            if (res != null)
            {
                return res.Type;
            }


            throw new Exception("WhatsEvent");
        }

        /// <summary>
        /// Проверка что за клад
        /// </summary>
        public static Treasure CheckChest(Coordinate coord, Lobby lobby)
        {
            var res = lobby.Chests.Find(e => Equals(e.Position, coord));
            return res;
        }
    }
}
