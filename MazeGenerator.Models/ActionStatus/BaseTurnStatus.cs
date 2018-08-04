namespace MazeGenerator.Models.ActionStatus
{
    public class BaseTurnStatus
    {
        public bool IsOtherTurn { get; set; }
        public bool IsGameEnd { get; set; }
        public Player CurrentPlayer { get; set; }
    }
}