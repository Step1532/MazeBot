using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGenerator.NewGame;
using Newtonsoft.Json;

namespace MazeGenerator.Models
{
    public class Lobby
    {
        public bool[,] Maze; 
        public List<Player> Players;
        public LobbyRules Rules;
        public List<GameEvent> Events;
        public int GameId;

        public Lobby(int gameId)
        {
            GameId = gameId;
            Rules = LobbyRules.GenerateTemplateRules();
        }
        public void Save()
        {
            Directory.CreateDirectory($@"\Game{GameId}");
            File.WriteAllText($@"\Game{GameId}\maze.json", JsonConvert.SerializeObject(Maze));
            File.WriteAllText($@"\Game{GameId}\CoordinateEvents.json", JsonConvert.SerializeObject(Rules));
            File.WriteAllText($@"\Game{GameId}\Players.json", JsonConvert.SerializeObject(Players));
        }

        public void Load()
        {
            Maze = JsonConvert.DeserializeObject<bool[,]>(File.ReadAllText($@"\Game{GameId}\maze.json"));
            Players = JsonConvert.DeserializeObject<List<Player>>(File.ReadAllText($@"\Game{GameId}\Players.json"));
            Rules = JsonConvert.DeserializeObject<LobbyRules>(File.ReadAllText($@"\Game{GameId}\CoordinateEvents.json"));
        }
    }
}
