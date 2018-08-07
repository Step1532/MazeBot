namespace MazeGenerator.Models
{
    public class Treasure
    {
        public Coordinate Position { get; set; }
        public bool IsReal { get; set; }
        public int Id { get; set; }

        public Treasure(Coordinate position, bool isReal, int id)
        {
            Position = position;
            IsReal = isReal;
            Id = id;
        }
    }
}