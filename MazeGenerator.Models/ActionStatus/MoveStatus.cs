using System.Collections.Generic;
using MazeGenerator.Models.Enums;

namespace MazeGenerator.Models.ActionStatus
{
    public class MoveStatus : BaseTurnStatus
    {
        public List<Player> PlayersOnSameCell { get; set; }
        public List<PlayerAction> PlayerActions { get; set; }
    }
}