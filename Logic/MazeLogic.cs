using MazeGenerator.GameGenerator;
using MazeGenerator.MazeLogic;
using MazeGenerator.Models;
using MazeGenerator.Tools;

namespace MazeGenerator.Logic
{
    public static class MazeLogic
    {
        public static MazeObjectType TryMove(Lobby lobby, Player player, Direction direction) // возвращает можно ли идти в направлении
        {
            var coord = Coordinate.TargetCoordinate(player.Rotate, direction);
            var res = LobbyService.CheckLobbyCoordinate(player.UserCoordinate - coord, lobby);
            if (res == MazeObjectType.Void)
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
            else if (res == MazeObjectType.Event)
            {
                player.UserCoordinate.X -= coord.X;
                player.UserCoordinate.Y -= coord.Y;
                return MazeObjectType.Event;
            }
            else
                return MazeObjectType.Wall;
            return res;
        }

        public static Player Shoot(Lobby lobby, Player player, Direction direction) //  проверка может ли пуля попасть в игрока, если да возвращакт игрока
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
                return null;
            if (type == MazeObjectType.Player)
            {
                var p = lobby.Players.Find(e => Equals(e.UserCoordinate, bulletPosition));
                if (p.Health == 1)
                    lobby.Players.Remove(p);
                return p;
            }

            return null;
        }
    }
}