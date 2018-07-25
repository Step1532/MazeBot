using System;
using System.Collections.Generic;
using System.IO;
using MazeGenerator.Models;
using Newtonsoft.Json;

namespace MazeGenerator.Logic
{
    public class Lobby
    {
        public int GameId { get; }
        public List<GameEvent> Events { get; set; }
        public List<Treasure>  Chests { get; set; }
        public Byte[,] Maze { get; set; }
        public List<Player> Players { get; private set; }
        public LobbyRules Rules { get; private set; }

        private string MazeFile => $@"\Game{GameId}\maze.json";
        private string EventFile => $@"\Game{GameId}\CoordinateEvents.json";
        private string PlayerFile => $@"\Game{GameId}\Players.json";

        public Lobby(int gameId)
        {
            GameId = gameId;
            Rules = LobbyRules.GenerateTemplateRules();
            Events = new List<GameEvent>();
            Players = new List<Player>();
            Chests = new List<Treasure>();
        }

        public void Save()
        {
            Directory.CreateDirectory($@"\Game{GameId}");
            File.WriteAllText(MazeFile, JsonConvert.SerializeObject(Maze));
            File.WriteAllText(EventFile, JsonConvert.SerializeObject(Events));
            File.WriteAllText(PlayerFile, JsonConvert.SerializeObject(Players));
            //TODO: save rules
        }

        public void Load()
        {
            Maze = JsonConvert.DeserializeObject<Byte[,]>(File.ReadAllText(MazeFile));
            Players = JsonConvert.DeserializeObject<List<Player>>(File.ReadAllText(PlayerFile)) ?? new List<Player>();
            Events = JsonConvert.DeserializeObject<List<GameEvent>>(File.ReadAllText(EventFile));

            //TODO:
            Rules = null;
        }
    }
}