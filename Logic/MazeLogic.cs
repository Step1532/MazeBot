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
            if ((player.UserCoordinate - coord).X != -1 || (player.UserCoordinate - coord).Y != -1 || (player.UserCoordinate - coord).X != lobby.Maze.GetLength(1)+1 || (player.UserCoordinate - coord).Y != lobby.Maze.GetLength(0) + 1)
            {
                var res = LobbyService.CheckLobbyCoordinate(player.UserCoordinate - coord, lobby);

                if (res != MazeObjectType.Wall)
                {
                    player.UserCoordinate.X -= coord.X;
                    player.UserCoordinate.Y -= coord.Y;
                    //костыль, переделать под бота
                    if (res == MazeObjectType.Event)
                    {
                        var events = LobbyService.WhatsEvent(player.UserCoordinate, lobby);
                        if (events == "A ")
                        {
                            player.Bombs = 3;
                            player.Guns = 2;
                        }
                        //TODO: если будем делать пещеры
                        //  if (events == "H ")
                        //  {

                        //  }
                        if (events == "+ ")
                        {
                            player.Health = 3;
                        }

                        if (events == "C ")
                        {
                            player.chest = LobbyService.CheckChest(player.UserCoordinate, lobby);
                        }
                    }

                }

                return res;
            }
            else
            {
                return MazeObjectType.Wall;
            }
        }

        public static bool TryShoot(Lobby lobby, Player player, Direction direction) //  проверка может ли игрок выстрелить
        {
            if (player.Health > 1 && player.Guns > 1)
                Shoot(lobby, player, direction);
            return false;
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
                else
                    p.Health--;
                return p;
            }

            return null;
        }
        public static bool Bomb(Lobby lobby, Player player, Direction direction) //  взрыв стены
        {
            var coord = Coordinate.TargetCoordinate(player.Rotate, direction);
            if (lobby.Maze[player.UserCoordinate.X - coord.X, player.UserCoordinate.Y - coord.Y] == 1)
            {
                lobby.Maze[player.UserCoordinate.X - coord.X, player.UserCoordinate.Y - coord.Y] = 0;
                player.Bombs--;
                return true;
            }
            else
            {
                return false;
            }
                
        }
    }
}