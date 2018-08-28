using System;
using System.Collections.Generic;
using System.IO;
using MazeGenerator.Models;
using Newtonsoft.Json;

namespace MazeGenerator.Database
{
    public class LobbyRepository
    {
#if DEBUG
        private string LobbyFile(int id) => $@"C:\Users\Step1\Desktop\mazegen\GameFiles\Game{id}\lobby.json";
#else
                private const string LobbyFile(int id) = $@"GameFiles\Game{id}\lobby.json";
#endif
        private readonly string _connectionString;
        public LobbyRepository()
        {
            _connectionString = Config.ConnectionString;
        }

        public void Create(Lobby lobby)
        {
#if DEBUG
            Directory.CreateDirectory($@"C:\Users\Step1\Desktop\mazegen\GameFiles\Game{lobby.GameId}");
#else
            Directory.CreateDirectory($@"GameFiles\Game{lobby.GameId}");
#endif
            File.WriteAllText(LobbyFile(lobby.GameId), JsonConvert.SerializeObject(lobby));
        }

        public Lobby Read(int lobbyId)
        {
            if (File.Exists(LobbyFile(lobbyId)) == false)
                return null;
            return JsonConvert.DeserializeObject<Lobby>(File.ReadAllText(LobbyFile(lobbyId)));
        }

        public void Update(Lobby lobby)
        {
            Create(lobby);
        }

        public void Delete(int lobbyId)
        {
            File.Delete(LobbyFile(lobbyId));
        }
    }
}