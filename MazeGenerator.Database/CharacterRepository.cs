using System;
using System.Collections.Generic;
using MazeGenerator.Models;

namespace MazeGenerator.Database
{
    public class CharacterRepository
    {
        private string _connectionString;

        public CharacterRepository()
        {
            _connectionString = Config.ConnectionString;
        }

        public void Create()
        {
            throw new NotImplementedException();
        }

        public List<int> Read(int playerId)
        {
            throw new NotImplementedException();
        }

        public void Update(Player player)
        {
            throw new NotImplementedException();
        }

        public void Delete(int playerId)
        {
            throw new NotImplementedException();
        }
    }
}