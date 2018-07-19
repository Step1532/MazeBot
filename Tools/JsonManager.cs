using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGenerator.MazeLogic;
using MazeGenerator.NewGame;
using Newtonsoft.Json;
namespace MazeGenerator.Tools
{
    class JsonManager
    {
        //TODO: запись в Json

        public void WriteMazeToJson(Byte[,] maze, int Gameid)
        {
            string serialized = JsonConvert.SerializeObject(maze);
            Directory.CreateDirectory($"/Game{Gameid}");
            File.WriteAllText(string.Format($"/Game{Gameid}/maze.json"), serialized);
        }
        public void WritePlayersToJson(List<Player> players, int Gameid)
        {


        }
        public void WriteCoordinateEventsToJson(List<CoordinateEvents> players, int Gameid)
        {


        }
        public void WriteLobbiesPlayerCountToJson(List<CoordinateEvents> players, int Gameid)
        {


        }
    }
}
