using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGenerator.MazeLogic;
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
        public List<CoordinateEvents> GetMazeEventses(int GameId)
        {
            string json = File.ReadAllText(string.Format($@"\Game{GameId}\CoordinateEvents.json"));
            List<CoordinateEvents> e = JsonConvert.DeserializeObject<List<CoordinateEvents>>(json);
            return e;
        }
        public List<Player> GetPlayersList(int GameId)
        {
            string json = File.ReadAllText(string.Format($@"\Game{GameId}\Players.json"));
            List<Player> PlayersList = JsonConvert.DeserializeObject<List<Player>>(json);
            return PlayersList;
        }
        public List<int> GetLobbiesList()
        {
            string json = File.ReadAllText("lobbiesPlayerCount.json");
            List<int> lobbiesList = JsonConvert.DeserializeObject<List<int>>(json);
            return lobbiesList;
        }
    }
}
