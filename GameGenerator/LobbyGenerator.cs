﻿using System;
using System.Linq;
using MazeGenerator.MazeLogic;
using MazeGenerator.Models;
using MazeGenerator.Tools;

namespace MazeGenerator.GameGenerator
{
    public static class LobbyGenerator
    {
        public static Lobby GenerateLobby(Lobby lobby)
        {
            CreateNewMaze(lobby);
            GenerateEvents(lobby);
            return lobby;
        }

        private static void CreateNewMaze(Lobby lobby)
        {
            var newMaze = new Maze((ushort)lobby.Rules.Size.X, (ushort)lobby.Rules.Size.Y);
            newMaze.GenerateTWMaze_GrowingTree();
            
            var bytesMaze = newMaze.LineToBlock();

            lobby.Maze = bytesMaze;
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

        private static bool CheckCoordinateEvents(Lobby lobby, Coordinate newCoordinate)
        {
            if (lobby.Maze[newCoordinate.X, newCoordinate.Y] == 1)
                return false;

            return lobby.Events.Any(e => e.Position.Equals(newCoordinate));
        }
    }
}