using MazeGenerator.GameGenerator;

namespace MazeGenerator.Models
{
    public class Player
    {
        //TODO: probably remove
        public int PlayerId { get; set; }
        public Direction Rotate { get; set; }
        public Coordinate UserCoordinate { get; set; }
        public Treasure chest{ get; set; }
        public int Health { get; set; }
        public int Guns  { get; set; }
        public int Bombs { get; set; }
    }
}