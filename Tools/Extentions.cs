using System;
using MazeGenerator.GameGenerator;
using MazeGenerator.Models;

namespace MazeGenerator.Tools
{
    public static class Extentions
    {
        //TODO: rewrite
        public static Coordinate GenerateRandomPosition(this Byte[,] maze)
        {
            var rnd = new Random();
            int x, y;
            for (var i = 0; i < 100000; i++)
            {
                x = rnd.Next(0, maze.GetLength(0));
                y = rnd.Next(0, maze.GetLength(1));
                if (maze[x, y] == 0) return new Coordinate(x, y);
            }

            //Console.WriteLine("Error");
            return null;
        }
        public static Coordinate GetCoordinate(this Direction direction)
        {
            switch (direction)
            {
                case Direction.North: return new Coordinate(0, -1);
                case Direction.South: return new Coordinate(0,  1);
                case Direction.West:  return new Coordinate(-1, 0);
                case Direction.East:  return new Coordinate(1,  0);                 
            }
            throw new Exception("GetCoordinate");
        }
    }
}