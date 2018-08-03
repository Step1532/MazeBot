using MazeGenerator.Models.Enums;

namespace MazeGenerator.Models.ActionStatus
{
    public class AttackStatus
    {
        public bool IsOtherTurn { get; set; }
        public bool IsGameEnd { get; set; }
        public Player CurrentPlayer { get; set; }

        public AttackType Result { get; set; }
        public Player Target { get; set; }
        public bool ShootCount { get; set; }

    }
}
