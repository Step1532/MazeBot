using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGenerator.NewGame;
using MazeGenerator.Tools;
namespace MazeGenerator.Tools
{
    class ParseJsonManager
    {
        //TODO: методы которые парсят по разному
        public bool[,] GetMazeMap(int GameId)
        {
            bool[,] maze = null;
            //TODO: парс лабиринта
            return maze;
        }
        public List<Players> GetPlayersList(int GameId)
        {
            List<Players> PlayersList = new List<Players>();
            //TODO: Parse players
            return PlayersList;
        }
    }
}
