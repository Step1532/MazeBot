using System;
using MazeGenerator.Logic;

namespace MazeGenerator.Database
{
    public class LobbyRepository
    {
        private readonly string _connectionString;
        public LobbyRepository()
        {
            _connectionString = Config.ConnectionString;
        }

        public void Create(Lobby lobby)
        {
            throw new NotImplementedException();
        }

        public Lobby Read(int lobbyId)
        {
            throw new NotImplementedException();
        }

        public void Update(Lobby lobby)
        {
            throw new NotImplementedException();
        }

        //TODO: delete/clean?
        public void Delete(int lobbyId)
        {
            throw new NotImplementedException();
        }
    }
}