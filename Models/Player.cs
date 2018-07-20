using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MazeGenerator.MazeLogic;
using MazeGenerator.Models;
using MazeGenerator.Tools;

namespace MazeGenerator.NewGame
{
    public class Player
    {
        public int Playerid;
        public Direction Rotate;
        public int Userid;
        public Coordinate UserCoordinate;
    }
}
