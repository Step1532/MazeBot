﻿using System;
using System.Collections.Generic;
using System.IO;
using MazeGenerator.Models;
using Newtonsoft.Json;

namespace MazeGenerator.Database
{
    public class LobbyRepository
    {
        private string LobbyFile(int id) => $@"C:\Users\Step1\Desktop\mazegen\GameFiles\Game{id}\lobby.json";

        private readonly string _connectionString;
        public LobbyRepository()
        {
            _connectionString = Config.ConnectionString;
        }

        public void Create(Lobby lobby)
        {
            Directory.CreateDirectory($@"C:\Users\Step1\Desktop\mazegen\GameFiles\Game{lobby.GameId}");
            File.WriteAllText(LobbyFile(lobby.GameId), JsonConvert.SerializeObject(lobby));
        }

        public Lobby Read(int lobbyId)
        {
            return JsonConvert.DeserializeObject<Lobby>(File.ReadAllText(LobbyFile(lobbyId)));
        }

        public void Update(Lobby lobby)
        {
            Create(lobby);
        }

        public void Delete(int lobbyId)
        {
            //TODO: delete old files
            throw new NotImplementedException();
        }
    }
}