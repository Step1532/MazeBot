namespace MazeGenerator.Models
{
    public class Player
    {
        public int PlayerId { get; set; }
        public Route Rotate { get; set; }
        public Coordinate UserCoordinate { get; set; }
        public int UserId { get; set; }
    }
}