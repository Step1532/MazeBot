using System;
using MazeGenerator.GameGenerator;
using MazeGenerator.Tools;

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

        public static Coordinate TargetCoordinate(Direction rotate, Direction moveDirection)
        {
            switch (moveDirection)
            {
                case Direction.North:
                    return rotate.GetCoordinate();
                case Direction.South:
                    return Maze.OppositeDirection(rotate).GetCoordinate();
                case Direction.East:
                    byte route = (byte)(((byte)rotate) << 1);
                    return ((Direction)(route == 16 ? 1 : route)).GetCoordinate();
                case Direction.West:
                    return TargetCoordinate(Maze.OppositeDirection(rotate), Direction.East);
               
            }
            throw new Exception("TargetCoordinate");
        }

        public static Coordinate operator -(Coordinate left, Coordinate right)
        {
            var newX = left.X - right.X;
            var newY = left.Y - right.Y;
            return new Coordinate(newX, newY);
        }
    }
}