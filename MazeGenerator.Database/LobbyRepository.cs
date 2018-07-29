using System;
using System.Collections.Generic;
using System.IO;
using MazeGenerator.Models;
using Newtonsoft.Json;

namespace MazeGenerator.Database
{
    public class LobbyRepository
    {
        private string MazeFile(int id) => $@"\Game{id}\maze.json";
        private string EventFile(int id) => $@"\Game{id}\CoordinateEvents.json";
        private string PlayerFile(int id) => $@"\Game{id}\Players.json";
        private string ChestsFile(int id) => $@"\Game{id}\Chests.json";
        private string StrokeFile(int id) => $@"\Game{id}\Stroke.json";

        private readonly string _connectionString;
        public LobbyRepository()
        {
            _connectionString = Config.ConnectionString;
        }

        public void Create(Lobby lobby)
        {
            Directory.CreateDirectory($@"\Game{lobby.GameId}");
            File.WriteAllText(MazeFile(lobby.GameId), JsonConvert.SerializeObject(lobby.Maze));
            File.WriteAllText(EventFile(lobby.GameId), JsonConvert.SerializeObject(lobby.Events));
            File.WriteAllText(PlayerFile(lobby.GameId), JsonConvert.SerializeObject(lobby.Players));
            File.WriteAllText(StrokeFile(lobby.GameId), JsonConvert.SerializeObject(lobby.stroke));
            File.WriteAllText(ChestsFile(lobby.GameId), JsonConvert.SerializeObject(lobby.Chests));
        }

        public Lobby Read(int lobbyId)
        {
            //TODO: chests?
            return new Lobby(lobbyId)
            {
                Maze = JsonConvert.DeserializeObject<Byte[,]>(File.ReadAllText(MazeFile(lobbyId))),
                Players = JsonConvert.DeserializeObject<List<Player>>(File.ReadAllText(PlayerFile(lobbyId))) ?? new List<Player>(),
                Events = JsonConvert.DeserializeObject<List<GameEvent>>(File.ReadAllText(EventFile(lobbyId))),
                Chests = JsonConvert.DeserializeObject<List<Treasure>>(File.ReadAllText(ChestsFile(lobbyId))),
                stroke = JsonConvert.DeserializeObject<int>(File.ReadAllText(StrokeFile(lobbyId))),
            };
        }

        public void Update(Lobby lobby)
        {
            Create(lobby);
        }

        //TODO: delete/clean?
        public void Delete(int lobbyId)
        {
            throw new NotImplementedException();
        }
    }
}