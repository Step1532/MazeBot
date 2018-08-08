using MazeGenerator.Models.Enums;

namespace MazeGenerator.Models.ActionStatus
{
    public class AttackStatus : BaseTurnStatus
    {
        public AttackType Result { get; set; }
        public Player Target { get; set; }
        public KeyboardType KeyboardType { get; set; }
        public bool PickChest { get; set; }

    }
}
