using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGenerator.MazeLogic;
using MazeGenerator.Models;
using MazeGenerator.Tools;
using Newtonsoft.Json;
namespace MazeGenerator.Tools
{
    //TODO: remove this (see Lobby.Save)
    public static class ParseJsonManager
    {
        //public static bool[,] GetMazeMap(int gameId)
        //{
        //    string json = File.ReadAllText(string.Format($@"\Game{gameId}\maze.json"));
        //    bool[,] maze = JsonConvert.DeserializeObject<bool[,]>(json);
        //    return maze;
        //}
        //public static List<Coordinate> GetMazeEvents(int gameId)
        //{
        //    string json = File.ReadAllText(string.Format($@"\Game{gameId}\CoordinateEvents.json"));
        //    var e = JsonConvert.DeserializeObject<List<Coordinate>>(json);
        //    return e;
        //}
        //public static List<Player> GetPlayersList(int gameId)
        //{
        //    string json = File.ReadAllText(string.Format($@"\Game{gameId}\Players.json"));
        //    var playersList = JsonConvert.DeserializeObject<List<Player>>(json);
        //    return playersList;
        //}
    }
}
