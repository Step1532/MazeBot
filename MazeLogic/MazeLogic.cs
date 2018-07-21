using MazeGenerator.NewGame;
using MazeGenerator.Tools;

namespace MazeGenerator.MazeLogic
{
    //TODO: Same name class/namespace
    public class MazeLogic
    {
        //TODO: enum with direction
        //TODO: user forward/back
        //TODO: send Lobby and Player, not id
        public bool TryMove(int playerId, int gameId, Direction direction)
        {
            var player = ParseJsonManager
                .GetPlayersList(gameId)
                .Find(e => e.PlayerId == playerId);
            var maze = ParseJsonManager.GetMazeMap(gameId);

            //TODO: get new position from direction
            if (maze[player.UserCoordinate.X, player.UserCoordinate.Y - 1] == false)
            {
                player.UserCoordinate.Y--;
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