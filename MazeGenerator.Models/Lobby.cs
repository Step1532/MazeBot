using System;
using System.Collections.Generic;

namespace MazeGenerator.Models
{
    public class Lobby
    {
        public int GameId { get; }
        public List<GameEvent> Events { get; set; }
        public List<Treasure> Chests { get; set; }
        public Byte[,] Maze { get; set; }
        public List<Player> Players { get; set; }
        public LobbyRules Rules { get; private set; }
        public int stroke { get; set; }
        public bool IsActive { get; set; }

        public Lobby(int gameId)
        {
            GameId = gameId;
            Rules = LobbyRules.GenerateTemplateRules();
            Events = new List<GameEvent>();
            Players = new List<Player>();
            Chests = new List<Treasure>();
            IsActive = true;
        }

        public Lobby()
        {
            
        }
    }
}