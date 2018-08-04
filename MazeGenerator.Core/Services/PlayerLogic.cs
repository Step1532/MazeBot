﻿using System;
using System.Collections.Generic;
using System.Linq;
using MazeGenerator.Core.Tools;
using MazeGenerator.Models;
using MazeGenerator.Models.ActionStatus;
using MazeGenerator.Models.Enums;

namespace MazeGenerator.Core.Services
{
    public static class PlayerLogic
    {
        /// <summary>
        ///     Возвращает можно ли идти в направлении
        /// </summary>
        public static List<PlayerAction> TryMove(Lobby lobby, Player player, Direction direction)
        {
            var coord = Extensions.TargetCoordinate(player.Rotate, direction);
            var types = MazeLogic.CheckLobbyCoordinate(player.UserCoordinate - coord, lobby);
            Console.Clear();
            Console.WriteLine(player.UserCoordinate.X + " " + player.UserCoordinate.Y);
            if (types.Contains(MazeObjectType.Wall) || types.Contains(MazeObjectType.Space))
                return new List<PlayerAction> {PlayerAction.OnWall};

            var actions = new List<PlayerAction>();
            player.UserCoordinate -= coord;

            if (types.Contains(MazeObjectType.Event))
            {
                var events = MazeLogic.EventsOnCell(player.UserCoordinate, lobby);
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
                    if (player.Chest == null && player.Health == lobby.Rules.PlayerMaxHealth)
                    {
                        player.Chest = MazeLogic.PickChest(player.UserCoordinate, lobby, player);
                        actions.Add(PlayerAction.OnChest);
                    }
            }

            if (types.Contains(MazeObjectType.Player))
            {
                var p = lobby.Players.Find(e =>
                    Equals(e.UserCoordinate, player.UserCoordinate) && e.TelegramUserId != player.TelegramUserId);
                actions.Add(PlayerAction.MeetPlayer);
            }


            if (types.Contains(MazeObjectType.Exit) && player.Chest != null)
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
           
            return actions;
        }


        /// <summary>
        ///     проверка может ли игрок выстрелить
        /// </summary>
        public static AttackStatus TryShoot(Lobby lobby, Player player, Direction direction)
        {
            if (player.Health <= 1 || player.Guns < 1)
                return new AttackStatus
                {
                    Result = AttackType.NoAttack,
                    CurrentPlayer = player
                };

            return Shoot(lobby, player, direction);

        }

        /// <summary>
        ///     проверка может ли пуля попасть в игрока, если да возвращакт игрока
        /// </summary>
        private static AttackStatus Shoot(Lobby lobby, Player player, Direction direction)
        {
            var coord = Extensions.TargetCoordinate(player.Rotate, direction);
            var bulletPosition = new Coordinate(player.UserCoordinate.X, player.UserCoordinate.Y);
            List<MazeObjectType> type;
            do
            {
                type = MazeLogic.CheckLobbyCoordinate(bulletPosition - coord, lobby);
                bulletPosition -= coord;
            } while (!type.Contains(MazeObjectType.Player) && !type.Contains(MazeObjectType.Wall));

            player.Guns--;

            if (type.Contains(MazeObjectType.Wall))
            {
                return new AttackStatus
                {
                    CurrentPlayer = player,
                    Result = AttackType.NoTarget,
                    Target = null,
                    ShootCount = (player.Guns != 0)
                };
            }

            if (type.All(t => t != MazeObjectType.Player))
                throw new Exception("Shoot");

            var target = lobby.Players.Find(e => Equals(e.UserCoordinate, bulletPosition));

            if (target.Chest != null)
            {
                DropChest(lobby, target);
            }

            if (target.Health == 1)
                KillPlayer(lobby, target);
            else
                target.Health--;

            return new AttackStatus
            {
                CurrentPlayer = player,
                Result = AttackType.Hit,
                Target = target,
                ShootCount = (player.Guns != 0)
            };
        }

        /// <summary>
        ///     Взрыв стены
        /// </summary>
        public static BombResultType Bomb(Lobby lobby, Player player, Direction direction)
        {
            if (player.Bombs <= 0) return BombResultType.NoBomb;
            var coord = Extensions.TargetCoordinate(player.Rotate, direction);
            player.Bombs--;
            if (MazeLogic.CheckLobbyCoordinate(player.UserCoordinate - coord, lobby)
                .Contains(MazeObjectType.Wall))
            {
                lobby.Maze.Set(player.UserCoordinate - coord, 0);
                return BombResultType.Wall;
            }

            return BombResultType.Void;
        }

        public static AttackStatus Stab(Lobby lobby, Player player)
        {
            var stabResult = new AttackStatus
            {
                CurrentPlayer = player
            };
            var target = MazeLogic.PlayersOnCell(player, lobby)?.FirstOrDefault();
            if (target == null)
            {
                stabResult.Result = AttackType.NoAttack;
                return stabResult;
            }

            stabResult.Target = target;
            if (target.Health > 1)
            {
                target.Health--;
                stabResult.Result = AttackType.Hit;
                return stabResult;
            }

            lobby.Players.Remove(target);
            stabResult.Result = AttackType.Kill;
            return stabResult;
        }

        private static void DropChest(Lobby lobby, Player target)
        {
            lobby.Events.Add(new GameEvent(EventTypeEnum.Chest,
                new Coordinate(target.UserCoordinate)));
            target.Chest.Position = new Coordinate(target.UserCoordinate);
            target.Chest = null;
        }

        //TODO: Дописать логику при убийстве
        private static void KillPlayer(Lobby lobby, Player target)
        {
            lobby.Players.Remove(target);
        }
    }
}