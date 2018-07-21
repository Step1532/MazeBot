using System;
using MazeGenerator.Models;

namespace MazeGenerator.Tools
{
    public static class Extentions
    {
        //TODO: rewrite
        public static Coordinate GenerateRandomPosition(this bool[,] maze)
        {
            var rnd = new Random();
            int x, y;
            for (var i = 0; i < 100000; i++)
            {
                //TODO: check this
                x = rnd.Next(0, maze.GetLength(0));
                y = rnd.Next(0, maze.GetLength(1));
                if (!maze[x, y]) return new Coordinate(x, y);
            }

            //Console.WriteLine("Error");
            return null;
        }
    }
}