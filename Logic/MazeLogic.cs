using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using MazeGenerator.Enums;
using MazeGenerator.GameGenerator;
using MazeGenerator.Models;
using MazeGenerator.TeleBot;
using MazeGenerator.Tools;

namespace MazeGenerator.Logic
{
    public static class MazeLogic
    {
        /// <summary>  
        /// Возвращает можно ли идти в направлении
        /// </summary>  
        public static string TryMove(Lobby lobby, Player player, Direction direction)
        {
            string Answer = "";
            var coord = Coordinate.TargetCoordinate(player.Rotate, direction);
            if ((player.UserCoordinate - coord).X != -1 || (player.UserCoordinate - coord).Y != -1 || (player.UserCoordinate - coord).X != lobby.Maze.GetLength(1)+1 || (player.UserCoordinate - coord).Y != lobby.Maze.GetLength(0) + 1)
            {
                var res = LobbyService.CheckLobbyCoordinate(player.UserCoordinate - coord, lobby);
                if (res[0] != MazeObjectType.Wall)
                {
                    player.UserCoordinate.X -= coord.X;
                    player.UserCoordinate.Y -= coord.Y;
                    for (int i = 0; i < res.Count; i++)
                    {
                        if (res[i] == MazeObjectType.Void)
                            Answer += "прошел";
                        if (res[i] == MazeObjectType.Event)
                        {
                            var events = LobbyService.WhatsEvent(player.UserCoordinate, lobby);
                            if (events == EventTypeEnum.Arsenal)
                            {
                                player.Bombs = 3;
                                player.Guns = 2;
                                Answer += Answers.GenerateArsenalAnswer(player);
                            }
                            //TODO: если будем делать пещеры
                            //  if (events == "H ")
                            //  {

                            //  }
                            if (events == EventTypeEnum.Hospital)
                            {
                                player.Health = 3;
                                Answer += Answers.GenerateHospitalAnswer(player);
                            }

                            //TODO: think about C and C|
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
                                       Answer += Answers.GenerateChestAnswer(player);
                                    }
                                }
                            }
                           
                        }
                        if (res[i] == MazeObjectType.Player)
                        {
                            var p = lobby.Players.Find(e => e.UserCoordinate == player.UserCoordinate && e.PlayerId != player.PlayerId);
                            Answer += Answers.GeneratePlayerAnswer(player, p);
                        }


                        if (res[i] == MazeObjectType.Exit)
                        {
                            var isReal = player.Chest ?? null;
                            if (isReal != null)
                            {
                                if (player.Chest.IsReal == false)
                                {
                                    var r = lobby.Chests.Find(e =>
                                        Equals(player.UserCoordinate, e.Position));
                                    lobby.Chests.Remove(r);
                                    player.Chest = null;
                                    Answer += Answers.GenerateChestAnswer(player);
                                }
                                else
                                {
                                    Answer += Answers.GenerateEndAnswer(player);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Answer += Answers.GenerateWallAnswer(player);
            }

            return Answer;
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
            var coord = Coordinate.TargetCoordinate(player.Rotate, direction);
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