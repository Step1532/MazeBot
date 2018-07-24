using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGenerator.MazeLogic;
using MazeGenerator.Models;

namespace MazeGenerator.Logic
{
    public class LobbyService
    {
        //TODO: вынести из лобби логику лоад и сейв сюда, лобби в модели
        public static MazeObjectType CheckLobbyCoordinate(Coordinate coord, Lobby lobby) // проверка что находится в клетке
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
        public static string WhatsEvent(Coordinate coord, Lobby lobby) // проверка что находится в клетке
        {
            var res = lobby.Events.Find(e => Equals(e.Position, coord));
            if (res.Type == EventTypeEnum.Arsenal)
                return "A ";
            if (res.Type == EventTypeEnum.Holes)
                return "H ";
            if (res.Type == EventTypeEnum.Hospital)
                return "+ ";
            if (res.Type == EventTypeEnum.Exit)
                return "E ";
            if (res.Type == EventTypeEnum.Chest)
                return "C ";



            throw new Exception("WhatsEvent");
        }
        public static Treasure CheckChest(Coordinate coord, Lobby lobby) // проверкачто за клад
        {
            var res = lobby.Chests.Find(e => Equals(e.Position, coord));
            return res;

            throw new Exception("WhatsEvent");
        }
    }
}
