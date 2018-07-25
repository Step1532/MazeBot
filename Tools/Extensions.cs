using System;
using System.Diagnostics;
using MazeGenerator.GameGenerator;
using MazeGenerator.Models;

namespace MazeGenerator.Tools
{
    public static class Extensions
    {
        private static readonly Random Rnd = new Random();

        //TODO: rewrite
        public static Coordinate GenerateRandomPosition(this Byte[,] maze)
        {
            while (true)
            {
                var x = Rnd.Next(maze.GetLength(0));
                var y = Rnd.Next(maze.GetLength(1));
                if (maze[x, y] == 0) return new Coordinate(x, y);
            }
        }

        public static Coordinate GetCoordinate(this Direction direction)
        {
            switch (direction)
            {
                case Direction.North: return new Coordinate(0,  1);
                case Direction.South: return new Coordinate(0, -1);
                case Direction.West:  return new Coordinate(-1, 0);
                case Direction.East:  return new Coordinate(1,  0);                 
            }
            throw new Exception("GetCoordinate");
        }
    }
}