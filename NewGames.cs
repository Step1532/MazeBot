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
    class NewGames
    {
        public Rules ruls = new Rules();
        public LobbyControl a = new LobbyControl();
        public Player player = new Player();
        public ParseJsonManager PJson = new ParseJsonManager();
        public JsonManager MJson = new JsonManager();

        public Lobby lobby; 
        //TODO: возможно стоит передавать чат id и через метод преобраззовать уже в лобби
        public void StartGame(int lobbyid)
        {
            lobby.GameId = lobbyid;
            //TODO: load rules
            GetNewMaze();
            GenerateEvents();
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
        public static void GetNewMaze()
        {
            ushort h, w;
            Random rnd = new Random();
            int tmp;
            tmp = rnd.Next(10, 20);
            h = (ushort)tmp;
            tmp = rnd.Next(10, 20);
            w = (ushort)tmp;
            Maze maze1 = new Maze(h, w);
            maze1.dumpMaze();

            List<UInt16[]> gt_output = maze1.GenerateTWMaze_GrowingTree();
            maze1.dumpMaze();

            Byte[,] blockmaze = maze1.LineToBlock();
            //TODO: хрень, переписать!!!!!!!!!!!
//            lobby.Maze = blockmaze;
           // e.WriteMazeToJson(blockmaze, Gameid, listEvents);
           //Console.WriteLine(string.Format($"new maze is created in lobby{Gameid}"));
        }

        public static void GenerateEvents()
        {
            List<Coordinate> listEvents = new List<Coordinate>();
            Coordinate a;
//            a = lobby.Maze.GenerateRandomPosition();
//            listEvents.Add(a);
            //TODO: считывание с правил в цикле 
        }
    }
}
