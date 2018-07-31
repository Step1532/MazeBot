using System;
using System.Collections.Generic;
using MazeGenerator.Models;

namespace MazeGenerator.Database
{
    public class 
        Repository
    {
        private readonly string _connectionString;

        public void PlayerRepository()
        {
      //      _connectionString = Config.ConnectionString;
        }

        public void Create(Player player)
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