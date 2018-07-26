using System;

namespace MazeGenerator.Models.Enums
{
    [Flags]
    public enum Direction : byte
    {
        North = 0x1,
        West = 0x2,
        South = 0x4,
        East = 0x8
    }
}