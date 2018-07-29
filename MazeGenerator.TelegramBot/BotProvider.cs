using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MazeGenerator.Core.Services;
using MazeGenerator.Core.Tools;
using MazeGenerator.Database;
using MazeGenerator.Models;
using MazeGenerator.Models.Enums;

namespace MazeGenerator.TelegramBot
{
    public static class BotProvider
    {
        public static string ShootCommand(long chatId, Direction direction, string username)
        {
            LobbyRepository repository = new LobbyRepository();
            Lobby lobby = repository.Read(LobbyControl.GetLobbyId(chatId));
            if (MazeLogic.TryShoot(lobby.Players[lobby.stroke]))
            {
                var res = MazeLogic.Shoot(lobby, lobby.Players[lobby.stroke], direction);
                repository.Update(lobby);
                if (res.Item2 != null)
                {
                    if (res.Item1 == ResultShoot.Kill)
                    {
                        return string.Format(Answers.ShootKill.RandomAnswer(), username);
                    }
                    return string.Format(Answers.ShootHit.RandomAnswer(), username);
                }
                else
                {
                    return string.Format(Answers.ShootWall.RandomAnswer(), username);
                }
            }
            return string.Format(Answers.NotBullet.RandomAnswer(), username);
        }
        public static string BombCommand(long chatId, Direction direction, string username)
        {
            LobbyRepository repository = new LobbyRepository();
            Lobby lobby = repository.Read(LobbyControl.GetLobbyId(chatId));
            var res = MazeLogic.Bomb(lobby, lobby.Players[lobby.stroke], direction);
            if (res == ResultBomb.Wall)
            {
                repository.Update(lobby);
                return string.Format(Answers.ResultBombWall.RandomAnswer(), username);
            }
            if (res == ResultBomb.NoBomb)
            {
                repository.Update(lobby);
                return string.Format(Answers.ResultBombNoBomb.RandomAnswer(), username);
            }
            if (res == ResultBomb.Void)
            {
                repository.Update(lobby);
                return string.Format(Answers.ResultBombVoid.RandomAnswer(), username);
            }
            return "";
        }

        public static string MoveCommand(long chatId, Direction direction, string username)
        {
            string answ = "";
            LobbyRepository repository = new LobbyRepository();
            Lobby lobby = repository.Read(LobbyControl.GetLobbyId(chatId));
            var res = MazeLogic.TryMove(lobby, lobby.Players[lobby.stroke], direction);
            List<string> ls = new List<string>();

            if (res.Contains(PlayerAction.GameEnd))
            {
                lobby.IsActive = false;
                ls.Add(string.Format(Answers.EndGame.RandomAnswer(), username));
            }
            else
            {
                foreach (var item in res)
                {
                    if (item == PlayerAction.OnWall)
                    {
                        ls.Add(string.Format(Answers.MoveWall.RandomAnswer(), username));
                    }

                    if (item == PlayerAction.FakeChest)
                    {
                        ls.Add(string.Format(Answers.ExitFalseChest.RandomAnswer(), username));
                    }

                    if (item == PlayerAction.MeetPlayer)
                    {
                        //TODO:
                        ls.Add(string.Format(Answers.ExitFalseChest.RandomAnswer(), username));
                    }
                    if (item == PlayerAction.OnArsenal)
                    {
                        ls.Add(string.Format(Answers.MoveArs.RandomAnswer(), username));
                    }
                    if (item == PlayerAction.OnChest)
                    {
                        ls.Add(string.Format(Answers.MoveChest.RandomAnswer(), username));
                    }
                    if (item == PlayerAction.OnHospital)
                    {
                        ls.Add(string.Format(Answers.MoveHosp.RandomAnswer(), username));
                    }
                }
            }
            repository.Update(lobby);
            return string.Join("\n", ls);
        }
    }
}
