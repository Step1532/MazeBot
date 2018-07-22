using System.Collections.Generic;
using System.IO;
using MazeGenerator.Models;
using Newtonsoft.Json;

namespace MazeGenerator.MazeLogic
{
    public class Lobby
    {
        public int GameId { get; }
        public List<GameEvent> Events { get; private set; }
        public bool[,] Maze { get; set; }
        public List<Player> Players { get; private set; }
        public LobbyRules Rules { get; private set; }

        private string MazeFile => $@"\Game{GameId}\maze.json";
        private string EventFile => $@"\Game{GameId}\CoordinateEvents.json";
        private string PlayerFile => $@"\Game{GameId}\Players.json";

        public Lobby(int gameId)
        {
            GameId = gameId;
            Rules = LobbyRules.GenerateTemplateRules();
        }

        public void Save()
        {
            Directory.CreateDirectory($@"\Game{GameId}");
            File.WriteAllText(MazeFile, JsonConvert.SerializeObject(Maze));
            File.WriteAllText(EventFile, JsonConvert.SerializeObject(Rules));

            //TODO: неправильное сохр игроков?
            //var playersList = new List<Player>();
            //var jsonString = File.ReadAllText($@"\Game{GameId}\Players.json");
            //playersList = JsonConvert.DeserializeObject<List<Player>>(jsonString) ?? new List<Player>();
            //playersList.Add(players);

            File.WriteAllText(PlayerFile, JsonConvert.SerializeObject(Players));
            //TODO: save events
        }

        public void Load()
        {
            //TODO: Посмотреть про  ?? new List<Player>(), возможно использовать
            Maze = JsonConvert.DeserializeObject<bool[,]>(File.ReadAllText(MazeFile));
            Players = JsonConvert.DeserializeObject<List<Player>>(File.ReadAllText(PlayerFile));
            Events = JsonConvert.DeserializeObject<List<GameEvent>>(File.ReadAllText(EventFile));

            //TODO:
            Rules = null;
        }
    }
}