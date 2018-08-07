namespace MazeGenerator.Models.ActionStatus
{
    public class BombStatus : BaseTurnStatus
    {
        public BombResultType Result { get; set; }
        public bool BombCount { get; set; }
    }
}