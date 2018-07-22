using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MazeGenerator.Models
{
    public class Lobby
    {
        public int GameId { get; }
        public List<GameEvent> Events { get; private set; }
        public bool[,] Maze { get; set; }
        public List<Player> Players { get; private set; }
        public LobbyRules Rules { get; private set; }

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
            //TODO: неправильное сохр игроков?
            //    var PlayersList = new List<Player>();
            //    var json = File.ReadAllText(string.Format($@"\Game{Gameid}\Players.json"));
            //    PlayersList = JsonConvert.DeserializeObject<List<Player>>(json);
            //    if (PlayersList == null)
            //    {
            //        PlayersList = new List<Player>();
            //        PlayersList.Add(players);
            //        var serialized = JsonConvert.SerializeObject(PlayersList);
            //        Directory.CreateDirectory($@"\Game{Gameid}");
            //        File.WriteAllText(string.Format($@"\Game{Gameid}\Players.json"), serialized);
            //    }
            //    else
            //    {
            //        PlayersList.Add(players);
            //        var serialized = JsonConvert.SerializeObject(PlayersList);
            //        Directory.CreateDirectory($@"\Game{Gameid}");
            //        File.WriteAllText(string.Format($@"\Game{Gameid}\Players.json"), serialized);
            //    }

            File.WriteAllText($@"\Game{GameId}\Players.json", JsonConvert.SerializeObject(Players));
            //TODO: save events
        }

        public void Load()
        {
            Maze = JsonConvert.DeserializeObject<bool[,]>(File.ReadAllText($@"\Game{GameId}\maze.json"));
            Players = JsonConvert.DeserializeObject<List<Player>>(File.ReadAllText($@"\Game{GameId}\Players.json"));
            Rules = JsonConvert.DeserializeObject<LobbyRules>(
                File.ReadAllText($@"\Game{GameId}\CoordinateEvents.json"));
            //TODO:
            Events = null;
        }
    }
}