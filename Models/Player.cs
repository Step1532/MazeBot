using MazeGenerator.GameGenerator;

namespace MazeGenerator.Models
{
    public class Player
    {
        //TODO: probably remove
        public int PlayerId { get; set; }
        public Direction Rotate { get; set; }
        public Coordinate UserCoordinate { get; set; }
    }
}