using System;
using System.Collections.Generic;

namespace MazeGenerator.Database
{
    public class MemberRepository
    {
        private readonly string _connectionString;

        public MemberRepository()
        {
            _connectionString = Config.ConnectionString;
        }

        public void Create(int lobbyId, int playerId)
        {
            throw new NotImplementedException();
        }

        public List<int> Read(int lobbyId)
        {
            throw new NotImplementedException();
        }

        public void Delete(int lobbyId, int playerId)
        {
            throw new NotImplementedException();
        }
    }
}