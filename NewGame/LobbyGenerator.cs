using System;
using System.Linq;
using MazeGenerator.Models;
using MazeGenerator.Tools;

namespace MazeGenerator.NewGame
{
    public static class LobbyGenerator
    {
        //TODO: возможно стоит передавать чат id и через метод преобраззовать уже в лобби
        public static Lobby GenerateLobby(int lobbyId)
        {
            var lobby = new Lobby(lobbyId);
            lobby.Maze = GetNewMaze(lobby.Rules.Size);
            GenerateEvents(lobby);
            return lobby;
        }

        //public string CheckStartGame(int countPlayers, int lobbyid)
        //{
        //    //TODO: Delete any user strings in this class
        //    if (countPlayers == ruls.RulesList[0])
        //    {
        //        GenerateLobby(lobbyid);
        //        return "Новая игра начата";
        //    }

        //    return string.Format($"Ожидание новых игроков, не хватает {ruls.RulesList[0] - countPlayers}");
        //}

        private static bool[,] GetNewMaze(Coordinate size)
        {
            var newMaze = new Maze((ushort) size.X, (ushort) size.Y);

            //TODO: check if x/y in correct order
            var bytesMaze = newMaze.LineToBlock();
            var finalMaze = new bool[size.X, size.Y];
            Array.Copy(bytesMaze, finalMaze, bytesMaze.Length);
            return finalMaze;
        }

        private static void GenerateEvents(Lobby lobby)
        {
            for (var i = 0; i < lobby.Rules.ArsenalCount; i++)
                AddEvent(EventTypeEnum.Arsenal, lobby);
            for (var i = 0; i < lobby.Rules.ExitCount; i++)
                AddEvent(EventTypeEnum.Exit, lobby);
            for (var i = 0; i < lobby.Rules.FalseGoldCount; i++)
                AddEvent(EventTypeEnum.FalseGoldCount, lobby);
            for (var i = 0; i < lobby.Rules.HolesCount; i++)
                AddEvent(EventTypeEnum.Holes, lobby);
            for (var i = 0; i < lobby.Rules.HospitalCount; i++)
                AddEvent(EventTypeEnum.Hospital, lobby);
            //TODO: считывание с правил в цикле 
        }

        private static void AddEvent(EventTypeEnum eventType, Lobby lobby)
        {
            Coordinate coordinate;
            do
            {
                coordinate = lobby.Maze.GenerateRandomPosition();
            } while (CheckCoordinateEvents(lobby, coordinate));

            lobby.Events.Add(new GameEvent {Position = coordinate, Type = eventType});
        }

        private static bool CheckCoordinateEvents(Lobby lobby, Coordinate newCoordinate)
        {
            //TODO: check if x/y in correct order
            if (lobby.Maze[newCoordinate.X, newCoordinate.Y]) return false;

            return lobby.Events.Any(e => e.Position.Equals(newCoordinate));
        }
    }
}