using MazeGenerator.Models;
using MazeGenerator.Tools;

namespace MazeGenerator.MazeLogic
{
    //TODO: Same name class/namespace
    public class MazeLogic
    {
        //TODO: enum with direction
        //TODO: user forward/back
        public bool TryMove(Lobby lobby, Player player, Route direction)
        {
            //TODO: get new position from direction
            var coord = direction.GetCoordinate();

            if (lobby.Maze[player.UserCoordinate.X - coord.X, player.UserCoordinate.Y - coord.Y] == false)
            {
                player.UserCoordinate.Y--;
                lobby.Save();
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