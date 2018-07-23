using MazeGenerator.GameGenerator;
using MazeGenerator.MazeLogic;
using MazeGenerator.Models;
using MazeGenerator.Tools;

namespace MazeGenerator.Logic
{
    public static class MazeLogic
    {
        public static bool TryMove(Lobby lobby, Player player, Direction direction)
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

        public static string Shoot(Lobby lobby, Player player, Direction direction)
        {
            var coord = Coordinate.TargetCoordinate(player.Rotate, direction);
            var bulletPosition = new Coordinate(player.UserCoordinate.X, player.UserCoordinate.Y);
            var type = LobbyService.CheckLobbyCoordinate(bulletPosition - coord, lobby);
            while (type == MazeObjectType.Void || type == MazeObjectType.Event)
            {
                type = LobbyService.CheckLobbyCoordinate(bulletPosition - coord, lobby);
                bulletPosition -= coord;
            }

            //TODO: fix return
            //TODO: create ActionType
            if (type == MazeObjectType.Wall)
                return "nothing";
            if (type == MazeObjectType.Player)
            {
                var p = lobby.Players.Find(e => Equals(e.UserCoordinate, bulletPosition));
                lobby.Players.Remove(p);
                return "Player";
            }
                
            return "";
        }
    }
}