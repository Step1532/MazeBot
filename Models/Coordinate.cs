namespace MazeGenerator.Models
{
    public class Coordinate
    {
        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Coordinate c) return X == c.X && Y == c.Y;
            return false;
        }

        //TODO: Coordinate TargetCoordinate(Direction rotate, Direction moveDirection)
    }
}