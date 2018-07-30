using System;
using System.Collections.Generic;
using System.Text;
using MazeGenerator.Models.Enums;

namespace MazeGenerator.Models
{
    public class ShootResult
    {
        public ResultShoot Result { get; set; }
        public Player Player { get; set; }
        public bool ShootCount { get; set; }

    }
}
