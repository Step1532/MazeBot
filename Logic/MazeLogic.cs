﻿using System.Linq;
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
                    if (player.chest != null)
                    {
                        player.chest.Position = player.UserCoordinate;
                    }
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

                        if (events == "C " || events == "C|")
                        {
                            //TODO:переделать что б можно было ронять на евенты
                            if (player.chest == null)
                            {
                                if (player.Health >= 3)
                                {
                                    player.chest = LobbyService.CheckChest(player.UserCoordinate, lobby);
                                    var tr = lobby.Events.Find(e => Equals(player.UserCoordinate, e.Position));
                                    lobby.Events.Remove(tr);
                                }
                            }
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
            while (type == MazeObjectType.Void)
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
                {
                    lobby.Players.Remove(p);
                }
                else
                {
                    p.Health--;
                }

                if (p.chest != null)
                {
                    lobby.Events.Add(new GameEvent(EventTypeEnum.Chest, p.UserCoordinate));
                    p.chest = null;
                }

                return p;
            }

            return null;
        }
        public static void Bomb(Lobby lobby, Player player, Direction direction) //  взрыв стены
        {
            if (player.Bombs > 0)
            {
                var coord = Coordinate.TargetCoordinate(player.Rotate, direction);
                if (lobby.Maze[player.UserCoordinate.X - coord.X, player.UserCoordinate.Y - coord.Y] == 1)
                {
                    lobby.Maze[player.UserCoordinate.X - coord.X, player.UserCoordinate.Y - coord.Y] = 0;
                }
                player.Bombs--;
            }
        }
    }
}