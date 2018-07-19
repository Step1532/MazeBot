using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGenerator.MazeLogic;
using MazeGenerator.NewGame;
using MazeGenerator.Tools;
namespace MazeGenerator.Tools
{
    public class ParseJsonManager
    {
        //TODO: методы которые парсят по разному
        public bool[,] GetMazeMap(int GameId)
        {
            bool[,] maze = null;
            //TODO: парс лабиринта
            return maze;
        }
        public CoordinateEvents GetMazeSize (int GameId)
        {
            CoordinateEvents e = new CoordinateEvents();
            //TODO: парс лабиринта
            return e;
        }
        public List<Players> GetPlayersList(int GameId)
        {
            List<Players> PlayersList = new List<Players>();
            //TODO: Parse players
            return PlayersList;
        }
        public List<int> GetLobbiesList()
        {
            List<int> lobbiesList = new List<int>();
            //TODO: Parse lobbies
            return lobbiesList;
        }
    }
}
