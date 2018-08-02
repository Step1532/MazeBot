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
            var types = LobbyService.CheckLobbyCoordinate(player.UserCoordinate - coord, lobby);
            Console.Clear();
            Console.WriteLine(player.UserCoordinate.X+ " " + player.UserCoordinate.Y);
            if (types.Contains(MazeObjectType.Wall) || types.Contains(MazeObjectType.Space))
            {
                return new List<PlayerAction> { PlayerAction.OnWall };
            }

            var actions = new List<PlayerAction>();
            player.UserCoordinate -= coord;
            
            if (types.Contains(MazeObjectType.Event))
            {
                var events = LobbyService.EventsOnCell(player.UserCoordinate, lobby);
                if (events.Contains(EventTypeEnum.Arsenal))
                {
                    player.Bombs = lobby.Rules.PlayerMaxBombs;
                    player.Guns = lobby.Rules.PlayerMaxGuns;
                    actions.Add(PlayerAction.OnArsenal);
                }

                if (events.Contains(EventTypeEnum.Hospital))
                {
                    player.Health = lobby.Rules.PlayerMaxHealth;
                    actions.Add(PlayerAction.OnHospital);
                }

                if (events.Contains(EventTypeEnum.Chest))
                {
                    if (player.Chest == null && player.Health == lobby.Rules.PlayerMaxHealth)
                    {
                        player.Chest = LobbyService.PickChest(player.UserCoordinate, lobby, player);
                        actions.Add(PlayerAction.OnChest);
                    }
                }
            }

            if (types.Contains(MazeObjectType.Player))
            {
                var p = lobby.Players.Find(e =>
                    Equals(e.UserCoordinate, player.UserCoordinate) && e.TelegramUserId != player.TelegramUserId);
                actions.Add(PlayerAction.MeetPlayer);
            }
            

            if (types.Contains(MazeObjectType.Exit))
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
        public static ShootResult TryShoot(Lobby lobby, Player player, Direction direction)
        {
            if (player.Health > 1 && player.Guns >= 1)
                return Shoot(lobby, player, direction);
            return null;
        }

        /// <summary>
        /// проверка может ли пуля попасть в игрока, если да возвращакт игрока
        /// </summary>
        private static ShootResult Shoot(Lobby lobby, Player player, Direction direction)
        {
            ShootResult res =new ShootResult();
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
            {
                res.Result = ResultShoot.Wall;
                res.Player = null;
                res.ShootCount = true;
                if (player.Guns == 0)
                {
                    res.ShootCount = false;
                }
                return res;

            }

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
                    res.Result = ResultShoot.Kill;
                    res.Player = p;
                    res.ShootCount = true;
                    if (player.Guns == 0)
                    {
                        res.ShootCount = false;
                    }
                    return res;
                }
                else
                {
                    p.Health--;
                    res.Result = ResultShoot.Hit;
                    res.Player = p;
                    res.ShootCount = true;
                    if (player.Guns == 0)
                    {
                        res.ShootCount = false;
                    }
                    return res;
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
        //TODO: stabresult
        public static Player Stab(Lobby lobby, Player player)
        {
            var res = LobbyService.PlayersOnCell(player, lobby)?.FirstOrDefault();
            if (res != null)
                res.Health--;
            if (res.Health == 1)
            {
                lobby.Players.Remove(res);
            }
            return res;
        }
    }
}