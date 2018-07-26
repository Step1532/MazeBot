using System;
using MazeGenerator.Models.Enums;

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

        

        public static Coordinate operator -(Coordinate left, Coordinate right)
        {
            var newX = left.X - right.X;
            var newY = left.Y - right.Y;
            return new Coordinate(newX, newY);
        }
    }
}