using System.Collections.Generic;
using System.Linq;
using MazeGenerator.Core.Tools;
using MazeGenerator.Models;
using MazeGenerator.Models.Enums;

namespace MazeGenerator.Core.GameGenerator
{
    public static class LobbyGenerator
    {
        public static void InitializeLobby(Lobby lobby)
        {
            CreateNewMaze(lobby);
            GenerateEvents(lobby);
            GeneratePlayers(lobby);
        }

        private static void GeneratePlayers(Lobby lobby)
        {
            foreach (var p in lobby.Players)
            {
                Coordinate coordinate;
                do
                {
                    coordinate = lobby.Maze.GenerateRandomPosition();
                } while (CheckCoordinateEvents(lobby, coordinate));

                p.UserCoordinate = coordinate;
            }
        }

        private static void CreateNewMaze(Lobby lobby)
        {
            lobby.Maze = Maze.CreateMaze((ushort) lobby.Rules.Size.X, (ushort) lobby.Rules.Size.Y);
        }

        private static void GenerateEvents(Lobby lobby)
        {
            lobby.Events = new List<GameEvent>();
            for (var i = 0; i < lobby.Rules.ArsenalCount; i++)
                AddEvent(EventTypeEnum.Arsenal, lobby);
            for (var i = 0; i < lobby.Rules.ExitCount; i++)
                AddEventExit(lobby);
            for (var i = 0; i < lobby.Rules.HolesCount; i++)
                AddEvent(EventTypeEnum.Holes, lobby);
            for (var i = 0; i < lobby.Rules.HospitalCount; i++)
                AddEvent(EventTypeEnum.Hospital, lobby);
            for (var i = 0; i < lobby.Rules.FalseGoldCount; i++)
                AddEventChest(lobby, false, 0);
            AddEventChest(lobby, true, 1);
        }

        private static void AddEvent(EventTypeEnum eventType, Lobby lobby)
        {
            Coordinate coordinate;
            do
            {
                coordinate = lobby.Maze.GenerateRandomPosition();
            } while (CheckCoordinateEvents(lobby, coordinate));

            lobby.Events.Add(new GameEvent(eventType, coordinate));
        }

        private static void AddEventExit(Lobby lobby)
        {
            Coordinate coordinate;
            do
            {
                coordinate = lobby.Maze.GenerateRandomPosition();
                coordinate.X = 1;
            } while (CheckCoordinateEvents(lobby, coordinate));

            coordinate.X--;
            lobby.Maze[coordinate.X, coordinate.Y] = 0;
        }

        private static void AddEventChest(Lobby lobby, bool Istrue, int id)
        {
            Coordinate coordinate;
            do
            {
                coordinate = lobby.Maze.GenerateRandomPosition();
            } while (CheckCoordinateEvents(lobby, coordinate));

            lobby.Events.Add(new GameEvent(EventTypeEnum.Chest, coordinate));
            lobby.Chests.Add(new Treasure(coordinate, Istrue, id));
        }

        /// <summary>
        ///     Проверка чтобы события не совпадали координатами
        /// </summary>
        /// <param name="lobby"></param>
        /// <param name="newCoordinate"></param>
        /// <returns></returns>
        private static bool CheckCoordinateEvents(Lobby lobby, Coordinate newCoordinate)
        {
            if (lobby.Maze[newCoordinate.X, newCoordinate.Y] == 1)
                return false;

            return lobby.Events.Any(e => e.Position.Equals(newCoordinate));
        }
    }
}