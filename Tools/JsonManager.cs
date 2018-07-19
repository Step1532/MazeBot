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

        public void WriteMazeToJson(Byte[,] maze, int Gameid, List<CoordinateEvents> eventses)
        {
            string serialized = JsonConvert.SerializeObject(maze);
            Directory.CreateDirectory($@"\Game{Gameid}");
            File.WriteAllText(string.Format($@"\Game{Gameid}\maze.json"), serialized);
            serialized = JsonConvert.SerializeObject(eventses);
            Directory.CreateDirectory($@"\Game{Gameid}");
            File.WriteAllText(string.Format($@"\Game{Gameid}\CoordinateEvents.json"), serialized);

        }
        public void WritePlayersToJson(List<Player> players, int Gameid)
        {
            string serialized = JsonConvert.SerializeObject(players);
            Directory.CreateDirectory($@"\Game{Gameid}");
            File.WriteAllText(string.Format($@"\Game{Gameid}\Players.json"), serialized);
        }
        public void WriteCoordinateEventsToJson(List<CoordinateEvents> eventses, int Gameid)
        {
            string serialized = JsonConvert.SerializeObject(eventses);
            Directory.CreateDirectory($@"\Game{Gameid}");
            File.WriteAllText(string.Format($@"\Game{Gameid}\CoordinateEvents.json"), serialized);
        }
        public void WriteLobbiesPlayerCountToJson(List<int> playerscount)
        {
            string serialized = JsonConvert.SerializeObject(playerscount);
            File.WriteAllText(string.Format("lobbiesPlayerCount.json"), serialized);
        }
    }
}
