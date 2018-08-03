using System.Collections.Generic;
using MazeGenerator.Models.Enums;

namespace MazeGenerator.Models.ActionStatus
{
    public class MoveStatus
    {
        public bool IsOtherTurn { get; set; }
        public bool IsGameEnd { get; set; }
        public Player CurrentPlayer { get; set; }
        
        public List<Player> PlayersOnSameCell { get; set; }
        public List<PlayerAction> PlayerActions { get; set; }
    }
}