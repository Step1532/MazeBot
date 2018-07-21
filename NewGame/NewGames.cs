using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGenerator.Models;
using MazeGenerator.NewGame;
using MazeGenerator.TeleBot;
using MazeGenerator.Tools;

namespace MazeGenerator
{
    public class NewGames
    {
        public Rules ruls = new Rules();
        public LobbyControl a = new LobbyControl();
        public Player player = new Player();

        public Lobby lobby;
        List<GameEvent> listEvents = new List<GameEvent>();

        //TODO: возможно стоит передавать чат id и через метод преобраззовать уже в лобби
        public void StartGame(int lobbyId)
        {
            lobby = new Lobby(lobbyId);
            //TODO: load rules
            lobby.Maze = GetNewMaze(lobby.Rules.Size);
            lobby.Events = GenerateEvents(lobby);

        }

        public string CheckStartGame(int countPlayers, int lobbyid)
        {
            if (countPlayers == ruls.RulesList[0])
            {
                StartGame(lobbyid);
                return "Новая игра начата";
            }

            return string.Format($"Ожидание новых игроков, не хватает {ruls.RulesList[0] - countPlayers}");
        }

        public static bool[,] GetNewMaze(Coordinate size)
        {
            Maze maze1 = new Maze((ushort) size.X, (ushort) size.Y);
            maze1.dumpMaze();

            List<UInt16[]> gt_output = maze1.GenerateTWMaze_GrowingTree();
            maze1.dumpMaze();

            //TODO: check if x/y in correct order
            Byte[,] blockmaze = maze1.LineToBlock();
            bool[,] finalMaze = new bool[size.X, size.Y];
            Array.Copy(blockmaze, finalMaze, blockmaze.Length);
            return finalMaze;

            //TODO: хрень, переписать!!!!!!!!!!!
            //            lobby.Maze = blockmaze;
            // e.WriteMazeToJson(blockmaze, Gameid, listEvents);
            //Console.WriteLine(string.Format($"new maze is created in lobby{Gameid}"));
        }

        public static List<GameEvent> GenerateEvents(Lobby lobby)
        {
            GameEvent events = new GameEvent();
            List<GameEvent> listEvents = new List<GameEvent>();
            Coordinate coordinate;
            //TODO: костыль
            for (int i = 0; i < lobby.Rules.ArsenalCount; i++)
            {
                events.Type = EventTypeEnum.Arsenal;
                events.Position = lobby.Maze.GenerateRandomPosition();

                while (CheckCoordinateEvents(events.Position))
                    events.Position = lobby.Maze.GenerateRandomPosition();

                listEvents.Add(events);
            }
            for (int i = 0; i < lobby.Rules.ExitCount; i++)
            {
                events.Type = EventTypeEnum.Exit;
                events.Position = lobby.Maze.GenerateRandomPosition();

                while (CheckCoordinateEvents(events.Position))
                    events.Position = lobby.Maze.GenerateRandomPosition();

                listEvents.Add(events);
            }
            for (int i = 0; i < lobby.Rules.FalseGoldCount; i++)
            {
                events.Type = EventTypeEnum.FalseGoldCount;
                events.Position = lobby.Maze.GenerateRandomPosition();

                while (CheckCoordinateEvents(events.Position))
                    events.Position = lobby.Maze.GenerateRandomPosition();

                listEvents.Add(events);
            }
            for (int i = 0; i < lobby.Rules.HolesCount; i++)
            {
                events.Type = EventTypeEnum.Holes;
                events.Position = lobby.Maze.GenerateRandomPosition();

                while (CheckCoordinateEvents(events.Position))
                    events.Position = lobby.Maze.GenerateRandomPosition();

                listEvents.Add(events);
            }
            for (int i = 0; i < lobby.Rules.HospitalCount; i++)
            {
                events.Type = EventTypeEnum.Hospital;
                events.Position = lobby.Maze.GenerateRandomPosition();

                while (CheckCoordinateEvents(events.Position))
                    events.Position = lobby.Maze.GenerateRandomPosition();

                listEvents.Add(events);
            }
            //TODO: считывание с правил в цикле
            return listEvents;
        }

        public static  bool CheckCoordinateEvents(Coordinate events)
        {
            //TODO: сдесь дожна быть ваша магия
            return listEvents.Exists();
        }
    }
}

