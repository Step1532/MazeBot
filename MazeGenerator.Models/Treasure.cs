namespace MazeGenerator.Models
{
    public class Treasure
    {
        public Coordinate Position { get; set; }
        public bool IsReal { get; set; }

        public Treasure(Coordinate position, bool isReal)
        {
            Position = position;
            IsReal = isReal;
        }
    }
}