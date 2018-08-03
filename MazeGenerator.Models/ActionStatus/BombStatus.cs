namespace MazeGenerator.Models.ActionStatus
{
    public class BombStatus
    {
        public bool IsOtherTurn { get; set; }
        public bool IsGameEnd { get; set; }
        public Player CurrentPlayer { get; set; }

        public BombResultType Result { get; set; }
    }
}