using System;
using System.Collections.Generic;
using System.Linq;
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

            //TODO: rename 
            var res = LobbyService.CheckLobbyCoordinate(player.UserCoordinate - coord, lobby);

            if (res.Contains(MazeObjectType.Wall) || res.Contains(MazeObjectType.Space))
            {
                return new List<PlayerAction> { PlayerAction.OnWall };
            }

            var actions = new List<PlayerAction>();
            player.UserCoordinate -= coord;
            
            if (res.Contains(MazeObjectType.Event))
            {
                var events = LobbyService.WhatsEvent(player.UserCoordinate, lobby);
                if (events == EventTypeEnum.Arsenal)
                {
                    //TODO: move to rules
                    player.Bombs = 3;
                    player.Guns = 2;
                    actions.Add(PlayerAction.OnArsenal);
                }

                if (events == EventTypeEnum.Hospital)
                {
                    player.Health = 3;
                    actions.Add(PlayerAction.OnHospital);
                }

                if (events == EventTypeEnum.Chest)
                {
                    //TODO: Создать константу в правилах PlayerMaxHitpoint
                    if (player.Chest == null && player.Health >= 3)
                    {
                        player.Chest = LobbyService.PickChest(player.UserCoordinate, lobby, player);
                        actions.Add(PlayerAction.OnChest);
                    }
                }
            }

            if (res.Contains(MazeObjectType.Player))
            {
                //TODO: think about it
                var p = lobby.Players.Find(e =>
                    Equals(e.UserCoordinate, player.UserCoordinate) && e.PlayerId != player.PlayerId);
                actions.Add(PlayerAction.MeetPlayer);
            }

            if (res.Contains(MazeObjectType.Exit))
            {
                if (player.Chest != null)
                {
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

            return actions;
        }


        /// <summary>
        /// проверка может ли игрок выстрелить
        /// </summary>
        public static bool TryShoot(Player player)
        {
            if (player.Health > 1 && player.Guns >= 1)
                return true;
            return false;
        }

        /// <summary>
        /// проверка может ли пуля попасть в игрока, если да возвращакт игрока
        /// </summary>
        public static (ResultShoot, Player) Shoot(Lobby lobby, Player player, Direction direction)
        {
            player.Guns--;
            var coord = Extensions.TargetCoordinate(player.Rotate, direction);
            var bulletPosition = new Coordinate(player.UserCoordinate.X, player.UserCoordinate.Y);
            List<MazeObjectType> type;
            do
            {
                type = LobbyService.CheckLobbyCoordinate(bulletPosition - coord, lobby);
                bulletPosition -= coord;
            } while (!type.Contains(MazeObjectType.Player) && !type.Contains(MazeObjectType.Wall));
            

            if (type.Contains(MazeObjectType.Wall))
                return (ResultShoot.Wall, null);

            if (type.Any(t => t == MazeObjectType.Player))
            {
                var p = lobby.Players.Find(e => Equals(e.UserCoordinate, bulletPosition));
                if (p.Chest != null)
                {
                    lobby.Events.Add(new GameEvent(EventTypeEnum.Chest,
                        new Coordinate(p.UserCoordinate.X, p.UserCoordinate.Y)));
                    p.Chest.Position = new Coordinate(p.UserCoordinate.X, p.UserCoordinate.Y);
                    p.Chest = null;
                }
                if (p.Health == 1)
                {
                    lobby.Players.Remove(p);
                    return (ResultShoot.Kill, p);
                }
                else
                {
                    p.Health--;
                    return (ResultShoot.Hit, p);
                }
            }

            throw new Exception("Shoot");
        }

        /// <summary>
        /// Взрыв стены
        /// </summary>
        public static ResultBomb Bomb(Lobby lobby, Player player, Direction direction)
        {
            if (player.Bombs <= 0)
            {
                return ResultBomb.NoBomb;
            }
            var coord = Extensions.TargetCoordinate(player.Rotate, direction);
            player.Bombs--;
            if (LobbyService.CheckLobbyCoordinate(player.UserCoordinate - coord, lobby)
                .Contains(MazeObjectType.Wall))
            {
                lobby.Maze.Set(player.UserCoordinate - coord, 0);
                return ResultBomb.Wall;
            }
            return ResultBomb.Void;
        }
    }
}