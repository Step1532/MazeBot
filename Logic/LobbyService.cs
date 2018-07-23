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
            if (coord.X == 0 || coord.Y == 0 || coord.X == lobby.Maze.GetLength(1) || coord.Y == lobby.Maze.GetLength(0))
                return MazeObjectType.Exit;
            if (lobby.Maze[coord.X, coord.Y] == 0)
                return MazeObjectType.Void;
            if (lobby.Maze[coord.X, coord.Y] == 1)
                return MazeObjectType.Wall;

            //TODO: create own exeption
            throw new Exception("CheckCoord");
        }
        public static string WhatsEvent(Coordinate coord, Lobby lobby) // проверка что находится в клетке
        {
            var res = lobby.Events.Find(e => Equals(e.Position, coord));
            if (res.Type == EventTypeEnum.Arsenal)
                return "Arsenal";
            if (res.Type == EventTypeEnum.Holes)
                return "Holes";
            if (res.Type == EventTypeEnum.Hospital)
                return "Hospitel";
            if (res.Type == EventTypeEnum.Exit)
                return "Exit";



            throw new Exception("WhatsEvent");
        }
    }
}
