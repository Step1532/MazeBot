using System;
using System.Diagnostics;
using MazeGenerator.GameGenerator;
using MazeGenerator.Models;

namespace MazeGenerator.Tools
{
    public static class Extentions
    {
        private static Random rnd = new Random();
        //TODO: rewrite
        public static Coordinate GenerateRandomPosition(this Byte[,] maze)
        {
            int x, y;
            for (var i = 0; i < 100000; i++)
            {
                x = rnd.Next(maze.GetLength(0));
                y = rnd.Next(maze.GetLength(1));
                Debug.Print(x + " " + y);
                if (maze[x, y] == 0) return new Coordinate(x, y);
            }

            //Console.WriteLine("Error");
            return null;
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