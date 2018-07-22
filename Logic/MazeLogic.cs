using MazeGenerator.GameGenerator;
using MazeGenerator.MazeLogic;
using MazeGenerator.Models;
using MazeGenerator.Tools;

namespace MazeGenerator.Logic
{
    public class MazeLogic
    {
        public bool TryMove(Lobby lobby, Player player, Direction direction)
        {
            var coord = Coordinate.TargetCoordinate(player.Rotate, direction);
            if (lobby.Maze[player.UserCoordinate.X - coord.X, player.UserCoordinate.Y - coord.Y] == 0)
            {
                player.UserCoordinate.X -= coord.X;
                player.UserCoordinate.Y -= coord.Y;
                return true;
            }
            return false;
        }

        public void Shoot(string Action, int playerId)
        {
            ;
        }
    }
}