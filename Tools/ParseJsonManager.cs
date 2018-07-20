using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGenerator.MazeLogic;
using MazeGenerator.Models;
using MazeGenerator.NewGame;
using MazeGenerator.Tools;
using Newtonsoft.Json;
namespace MazeGenerator.Tools
{
    public class ParseJsonManager
    {
        public bool[,] GetMazeMap(int GameId)
        {
            string json = File.ReadAllText(string.Format($@"\Game{GameId}\maze.json"));
            bool[,] maze = JsonConvert.DeserializeObject<bool[,]>(json);
            return maze;
        }
        public List<Coordinate> GetMazeEventses(int GameId)
        {
            string json = File.ReadAllText(string.Format($@"\Game{GameId}\CoordinateEvents.json"));
            List<Coordinate> e = JsonConvert.DeserializeObject<List<Coordinate>>(json);
            return e;
        }
        public List<Player> GetPlayersList(int GameId)
        {
            string json = File.ReadAllText(string.Format($@"\Game{GameId}\Players.json"));
            List<Player> PlayersList = JsonConvert.DeserializeObject<List<Player>>(json);
            return PlayersList;
        }
    }
}
