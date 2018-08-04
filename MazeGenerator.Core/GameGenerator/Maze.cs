using System;
using System.Collections.Generic;
using MazeGenerator.Core.Tools;
using MazeGenerator.Models.Enums;

namespace MazeGenerator.Core.GameGenerator
{
    public static class Maze
    {
        private static readonly Random Random = new Random((int) DateTime.Now.Ticks & 0x0000FFFF);

        public static byte[,] CreateMaze(ushort width, ushort height)
        {
            var maze = new byte[width, height];
            GenerateTWMaze_GrowingTree(maze);
            return LineToBlock(maze);
        }

        private static void GenerateTWMaze_GrowingTree(byte[,] maze)
        {
            var cells = new List<ushort[]>();

            ushort x = (byte) (Random.Next(maze.GetLength(0) - 1) + 1);
            ushort y = (byte) (Random.Next(maze.GetLength(1) - 1) + 1);

            cells.Add(new[] {x, y});

            while (cells.Count > 0)
            {
                var index = (short) ChooseIndex((ushort) cells.Count);
                var cellPicked = cells[index];

                x = cellPicked[0];
                y = cellPicked[1];

                foreach (var way in RandomizeDirection())
                {
                    var move = DoAStep(way);

                    var nx = (short) (x + move[0]);
                    var ny = (short) (y + move[1]);

                    if (nx >= 0 && ny >= 0 && nx < maze.GetLength(0) && ny < maze.GetLength(1) && maze[nx, ny] == 0)
                    {
                        maze[x, y] |= (byte) way;
                        maze[nx, ny] |= (byte) way.OppositeDirection();

                        cells.Add(new[] {(ushort) nx, (ushort) ny});

                        index = -1;
                        break;
                    }
                }

                if (index != -1) cells.RemoveAt(index);
            }
        }


        private static ushort ChooseIndex(ushort max)
        {
            ushort index = 0;

            if (max >= 1)
                index = (ushort)(max - 1);
            else
                index = 0;

            return index;
        }

        private static Direction[] RandomizeDirection()
        {
            var directions = (Direction[]) Enum.GetValues(typeof(Direction));
            Shuffle(directions);
            return directions;
        }

        // comes from http://www.dotnetperls.com/fisher-yates-shuffle
        private static void Shuffle<T>(T[] array)
        {
            var n = array.Length;

            for (var i = 0; i < n; i++)
            {
                var r = i + (int) (Random.NextDouble() * (n - i));
                var t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }


        private static sbyte[] DoAStep(Direction facingDirection)
        {
            sbyte[] step = {0, 0};

            switch (facingDirection)
            {
                case Direction.North:
                    step[0] = 0;
                    step[1] = -1;
                    break;
                case Direction.South:
                    step[0] = 0;
                    step[1] = 1;
                    break;
                case Direction.East:
                    step[0] = 1;
                    step[1] = 0;
                    break;
                case Direction.West:
                    step[0] = -1;
                    step[1] = 0;
                    break;
            }

            return step;
        }


        private static byte[,] LineToBlock(byte[,] maze)
        {
            if (maze == null || maze.GetLength(0) <= 1 && maze.GetLength(1) <= 1) return null;

            var lineToBlock = new byte[2 * maze.GetLength(0) + 1, 2 * maze.GetLength(1) + 1];

            for (ushort wall = 0; wall < 2 * maze.GetLength(1) + 1; wall++) lineToBlock[0, wall] = 1;
            for (ushort wall = 0; wall < 2 * maze.GetLength(0) + 1; wall++) lineToBlock[wall, 0] = 1;

            for (ushort y = 0; y < maze.GetLength(1); y++)
            for (ushort x = 0; x < maze.GetLength(0); x++)
            {
                lineToBlock[2 * x + 1, 2 * y + 1] = 0;

                if ((maze[x, y] & (byte) Direction.East) != 0)
                    lineToBlock[2 * x + 2, 2 * y + 1] = 0; // B
                else
                    lineToBlock[2 * x + 2, 2 * y + 1] = 1;

                if ((maze[x, y] & (byte) Direction.South) != 0)
                    lineToBlock[2 * x + 1, 2 * y + 2] = 0; // C
                else
                    lineToBlock[2 * x + 1, 2 * y + 2] = 1;


                lineToBlock[2 * x + 2, 2 * y + 2] = 1;
            }

            return lineToBlock;
        }
    }
}