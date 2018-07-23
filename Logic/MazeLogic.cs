using MazeGenerator.GameGenerator;
using MazeGenerator.MazeLogic;
using MazeGenerator.Models;
using MazeGenerator.Tools;

namespace MazeGenerator.Logic
{
    public static class MazeLogic
    {
        public static MazeObjectType TryMove(Lobby lobby, Player player, Direction direction)
        {
            var coord = Coordinate.TargetCoordinate(player.Rotate, direction);
            var res = LobbyService.CheckLobbyCoordinate(player.UserCoordinate - coord, lobby);
            if(res == MazeObjectType.Void)
            {
                player.UserCoordinate.X -= coord.X;
                player.UserCoordinate.Y -= coord.Y;
                if (player.UserCoordinate.X == 0 || player.UserCoordinate.Y == 0 ||
                    player.UserCoordinate.X == lobby.Maze.GetLength(1) ||
                    player.UserCoordinate.Y == lobby.Maze.GetLength(0))
                    return MazeObjectType.Exit;
                else
                    return MazeObjectType.Void;
            }
            else
                return MazeObjectType.Wall;
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