using System;
using System.Collections.Generic;
using System.Text;
using MazeGenerator.Models.ActionStatus;
using MazeGenerator.Models.Enums;

namespace MazeGenerator.Models
{
    public class StabResult
    {
        public AttackType Result { get; set; }
        public Player Player { get; set; }
    }
}
