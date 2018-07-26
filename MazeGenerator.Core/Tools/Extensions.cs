using System;
using MazeGenerator.Models;
using MazeGenerator.Models.Enums;

namespace MazeGenerator.Core.Tools
{
    public static class Extensions
    {
        private static readonly Random Rnd = new Random();

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
                case Direction.North: return new Coordinate(0, 1);
                case Direction.South: return new Coordinate(0, -1);
                case Direction.West: return new Coordinate(-1, 0);
                case Direction.East: return new Coordinate(1, 0);
            }
            throw new Exception("GetCoordinate");
        }

        public static Direction OppositeDirection(this Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return Direction.South;
                case Direction.South:
                    return Direction.North;
                case Direction.East:
                    return Direction.West;
                case Direction.West:
                    return Direction.East;
            }

            throw new ArgumentException();
        }

        public static Coordinate TargetCoordinate(Direction rotate, Direction moveDirection)
        {
            switch (moveDirection)
            {
                case Direction.North:
                    return rotate.GetCoordinate();
                case Direction.South:
                    return rotate.OppositeDirection().GetCoordinate();
                case Direction.East:
                    byte route = (byte)(((byte)rotate) << 1);
                    return ((Direction)(route == 16 ? 1 : route)).GetCoordinate();
                case Direction.West:
                    return TargetCoordinate(rotate.OppositeDirection(), Direction.East);

            }
            throw new Exception("TargetCoordinate");
        }
    }
}