using MazeGenerator.Models;
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
        public bool TryMove(Lobby lobby, Player player, Route direction)
        {
            //TODO: get new position from direction
            int a = 0, b = 0;
            //TODO: обдумать
            switch (direction)
            {
                case Route.North: a =  0; b = -1; break;
                case Route.East : a = -1; b =  0; break;
                case Route.South: a =  0; b =  1; break;
                case Route.Weast: a =  0; b =  0; break;
                
            }
            if (lobby.Maze[player.UserCoordinate.X - a, player.UserCoordinate.Y - b] == false)
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