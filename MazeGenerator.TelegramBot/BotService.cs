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
    public static class BotService
    {
        public static MessageConfig ShootCommand(int chatId, Direction direction, string username)
        {
            LobbyRepository repository = new LobbyRepository();
            Lobby lobby = repository.Read(LobbyControl.GetLobbyId(chatId));
            MessageConfig msg = new MessageConfig();
            var shootResult = MazeLogic.TryShoot(lobby, lobby.Players[lobby.stroke], direction);
            if (shootResult == null)
            {
                msg.Answer = string.Format(Answers.NotBullet.RandomAnswer(), username);
                msg.AnswerForOther = string.Format(Answers.NotBullet.RandomAnswer(), username);
                msg.KeyBoardId = KeyBoardEnum.Bomb;
                return msg;
            }
            else
            {
                repository.Update(lobby);
                if (shootResult.Player != null)
                {
                    if (shootResult.Result == ResultShoot.Kill)
                    {
                        msg.Answer = string.Format(Answers.ShootKill.RandomAnswer(), username);
                        msg.AnswerForOther = string.Format(Answers.ShootKill.RandomAnswer(), username);
                        if (shootResult.ShootCount == false)
                        {
                            msg.KeyBoardId = KeyBoardEnum.Bomb;
                        }
                    }

                    msg.Answer = string.Format(Answers.ShootHit.RandomAnswer(), username);
                    msg.AnswerForOther = string.Format(Answers.ShootHit.RandomAnswer(), username);
                    if (shootResult.ShootCount == false)
                    {
                        msg.KeyBoardId = KeyBoardEnum.Bomb;
                    }
                }
                else
                {
                    msg.Answer = string.Format(Answers.ShootWall.RandomAnswer(), username);
                    msg.AnswerForOther = string.Format(Answers.ShootWall.RandomAnswer(), username);
                    if (shootResult.ShootCount == false)
                    {
                        msg.KeyBoardId = KeyBoardEnum.Bomb;
                    }
                }
            }
            return msg;
        }

        public static MessageConfig BombCommand(long chatId, Direction direction, string username)
        {
            LobbyRepository repository = new LobbyRepository();
            Lobby lobby = repository.Read(LobbyControl.GetLobbyId(chatId));
            var res = MazeLogic.Bomb(lobby, lobby.Players[lobby.stroke], direction);
            repository.Update(lobby);

            if (res == ResultBomb.Wall)
            {
//                return string.Format(Answers.ResultBombWall.RandomAnswer(), username);
            }
            if (res == ResultBomb.NoBomb)
            {
//                return string.Format(Answers.ResultBombNoBomb.RandomAnswer(), username);
            }
            if (res == ResultBomb.Void)
            {
//                return string.Format(Answers.ResultBombVoid.RandomAnswer(), username);
            }

            throw new Exception();
        }

        public static MessageConfig MoveCommand(long chatId, Direction direction, string username)
        {
            

            LobbyRepository repository = new LobbyRepository();
            Lobby lobby = repository.Read(LobbyControl.GetLobbyId(chatId));
            var res = MazeLogic.TryMove(lobby, lobby.Players[lobby.stroke], direction);
            MessageConfig msg = new MessageConfig();

            bool isArsenal = false;
            List<string> ls = new List<string>();
            msg.KeyBoardId  = KeyBoardEnum.Move;
            if (res.Contains(PlayerAction.GameEnd))
            {
                lobby.IsActive = false;
                ls.Add(string.Format(Answers.EndGame.RandomAnswer(), username));
                return null;
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
                        //TODO:
                        msg.KeyBoardId = KeyBoardEnum.ShootwithBomb;
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
            msg.Answer = string.Join("\n", ls);
            msg.AnswerForOther = msg.Answer;
            return msg;
        }
    }
}
