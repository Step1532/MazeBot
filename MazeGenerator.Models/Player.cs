using System;
using MazeGenerator.Models.Enums;

namespace MazeGenerator.Models
{
    public class Player
    {
        public int TelegramUserId { get; set; }
        public string HeroName { get; set; }
        public Direction Rotate { get; set; }
        public Coordinate UserCoordinate { get; set; }
        public Treasure Chest { get; set; }
        public int Health { get; set; }
        public int Guns { get; set; }
        public int Bombs { get; set; }
    }
}