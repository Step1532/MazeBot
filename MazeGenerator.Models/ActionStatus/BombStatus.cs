using MazeGenerator.Models.Enums;

namespace MazeGenerator.Models.ActionStatus
{
    public class BombStatus : BaseTurnStatus
    {
        public BombResultType Result { get; set; }
        public KeyboardType KeyboardId { get; set; }
    }
}