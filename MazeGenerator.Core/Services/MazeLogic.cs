using System.Collections.Generic;
using MazeGenerator.Core.Tools;
using MazeGenerator.Models;
using MazeGenerator.Models.Enums;

namespace MazeGenerator.Core.Services
{
    public static class MazeLogic
    {
        /// <summary>  
        /// Возвращает можно ли идти в направлении
        /// </summary>  
        public static List<PlayerAction> TryMove(Lobby lobby, Player player, Direction direction)
        {
            var coord = Extensions.TargetCoordinate(player.Rotate, direction);
            if ((player.UserCoordinate - coord).X == -1 && (player.UserCoordinate - coord).Y == -1 &&
                (player.UserCoordinate - coord).X == lobby.Maze.GetLength(1) + 1 &&
                (player.UserCoordinate - coord).Y == lobby.Maze.GetLength(0) + 1)
            {
                return new List<PlayerAction>() { PlayerAction.OnWall };
            }
            else
            {
                var actions = new List<PlayerAction>();

                var res = LobbyService.CheckLobbyCoordinate(player.UserCoordinate - coord, lobby);
                if (res[0] != MazeObjectType.Wall)
                {
                    player.UserCoordinate.X -= coord.X;
                    player.UserCoordinate.Y -= coord.Y;
                    for (int i = 0; i < res.Count; i++)
                    {
                        //if (res[i] == MazeObjectType.Void)
                        //    //answer += "прошел";
                        //    ;
                        if (res[i] == MazeObjectType.Event)
                        {
                            var events = LobbyService.WhatsEvent(player.UserCoordinate, lobby);
                            if (events == EventTypeEnum.Arsenal)
                            {
                                player.Bombs = 3;
                                player.Guns = 2;
                                actions.Add(PlayerAction.OnArsenal);
                            }   
                            //TODO: если будем делать пещеры
                            //  if (events == "H ")
                            //  {

                            //  }
                            if (events == EventTypeEnum.Hospital)
                            {
                                player.Health = 3;
                                actions.Add(PlayerAction.OnHospital);
                            }

                            if (events == EventTypeEnum.Chest)
                            {
                                //TODO:переделать что б можно было ронять на евенты
                                if (player.Chest == null)
                                {
                                    if (player.Health >= 3)
                                    {
                                        player.Chest = LobbyService.CheckChest(player.UserCoordinate, lobby);
                                        var tr = lobby.Events.Find(e => Equals(player.UserCoordinate, e.Position));
                                        lobby.Events.Remove(tr);
                                        actions.Add(PlayerAction.OnChest);
                                    }
                                }
                            }
                        }

                        if (res[i] == MazeObjectType.Player)
                        {
                            var p = lobby.Players.Find(e =>
                                Equals(e.UserCoordinate, player.UserCoordinate) && e.PlayerId != player.PlayerId);
                            actions.Add(PlayerAction.MeetPlayer);
                        }


                        if (res[i] == MazeObjectType.Exit)
                        {
                            if (player.Chest == null) continue;

                            if (player.Chest.IsReal == false)
                            {
                                var r = lobby.Chests.Find(e =>
                                    Equals(player.UserCoordinate, e.Position));
                                lobby.Chests.Remove(r);
                                player.Chest = null;
                                actions.Add(PlayerAction.FakeChest);
                            }
                            else
                            {
                                actions.Add(PlayerAction.GameEnd);
                            }
                        }
                    }
                }

                return actions;
            }
        }

        /// <summary>
        /// проверка может ли игрок выстрелить
        /// </summary>
        public static bool TryShoot(Lobby lobby, Player player, Direction direction)
        {
            if (player.Health > 1 && player.Guns > 1)
                Shoot(lobby, player, direction);
            return false;
        }

        //TODO: А зачем оно возвращает игрока?
        /// <summary>
        /// проверка может ли пуля попасть в игрока, если да возвращакт игрока
        /// </summary>
        public static Player Shoot(Lobby lobby, Player player, Direction direction)
        {
            var coord = Extensions.TargetCoordinate(player.Rotate, direction);
            var bulletPosition = new Coordinate(player.UserCoordinate.X, player.UserCoordinate.Y);
            var type = LobbyService.CheckLobbyCoordinate(bulletPosition - coord, lobby);
            while (type[0] == MazeObjectType.Void)
            {
                type = LobbyService.CheckLobbyCoordinate(bulletPosition - coord, lobby);
                bulletPosition -= coord;
            }

            //TODO: fix return
            //TODO: create ActionType

            if (type[0] == MazeObjectType.Wall)
                return null;
            for (int i = 0; i < type.Count; i++)
            {
                if (type[i] == MazeObjectType.Player)
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

                    if (p.Chest != null)
                    {
                        lobby.Events.Add(new GameEvent(EventTypeEnum.Chest,
                            new Coordinate(p.UserCoordinate.X, p.UserCoordinate.Y)));
                        p.Chest = null;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Взрыв стены
        /// </summary>
        /// <param name="lobby"></param>
        /// <param name="player"></param>
        /// <param name="direction"></param>
        public static void Bomb(Lobby lobby, Player player, Direction direction)
        {
            if (player.Bombs > 0)
            {
                var coord = Extensions.TargetCoordinate(player.Rotate, direction);
                if (lobby.Maze[player.UserCoordinate.X - coord.X, player.UserCoordinate.Y - coord.Y] == 1)
                {
                    lobby.Maze[player.UserCoordinate.X - coord.X, player.UserCoordinate.Y - coord.Y] = 0;
                }

                player.Bombs--;
            }
        }
    }
}