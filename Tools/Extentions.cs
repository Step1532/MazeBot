using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGenerator.Models;

namespace MazeGenerator.Tools
{
    public static class Extentions
    {
        public static Coordinate GenerateRandomPosition(this bool[,] maze)
        {
            Random rnd = new Random();
            int x, y;
            for (int i = 0; i < 1000; i++)
            {
                //TODO: check this
                x = rnd.Next(0, maze.GetLength(0));
                y = rnd.Next(0, maze.GetLength(1));
                if (!maze[x, y])
                {
                    return new Coordinate(x, y);
                }
            }

            //Console.WriteLine("Error");
            return null;
        }
    }
}
