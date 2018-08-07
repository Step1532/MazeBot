using System;
using System.Collections.Generic;
using System.Text;

namespace MazeGenerator.Models.ActionStatus
{
    public class SkipTurnResult
    {
        public bool CanMakeTurn { get; set; }
        public bool PickChest { get; set; }
    }
}
