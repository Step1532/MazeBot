using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MazeGenerator.Core.GameGenerator;
using MazeGenerator.Core.Services;
using MazeGenerator.Core.Tools;
using MazeGenerator.Database;
using MazeGenerator.Models;
using MazeGenerator.Models.Enums;
using MazeGenerator.TelegramBot.Models;

namespace MazeGenerator.TelegramBot
{
    public static class BotService
    {
        public static LobbyRepository repository = new LobbyRepository();
        public static MemberRepository repo = new MemberRepository();
        public static MessageConfig ShootCommand(int chatId, Direction direction, string username)
        {
            Lobby lobby = repository.Read(repo.ReadLobbyId(chatId));
            MessageConfig msg = new MessageConfig();
            var shootResult = MazeLogic.TryShoot(lobby, lobby.Players[lobby.CurrentTurn], direction);
            repository.Update(lobby);

            if (shootResult == null)
            {
                msg.Answer = string.Format(Answers.NotBullet.RandomAnswer(), username);
                msg.AnswerForOther = string.Format(Answers.NotBullet.RandomAnswer(), username);
                msg.KeyBoardId = KeyBoardEnum.Bomb;
                return msg;
            }

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
            return msg;
        }

        public static MessageConfig BombCommand(int chatId, Direction direction, string username)
        {
            LobbyRepository repository = new LobbyRepository();
            Lobby lobby = repository.Read(repo.ReadLobbyId(chatId));
            var res = MazeLogic.Bomb(lobby, lobby.Players[lobby.CurrentTurn], direction);
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

        public static void StartGame(int playerId)
        {
            MemberRepository repo = new MemberRepository();
            var gameid = repo.ReadLobbyId(playerId);
            var players = repo.Read(gameid);
            Lobby lobby = new Lobby(gameid);
            foreach (var p in players)
            {
                Player player = new Player
                {
                    Rotate = Direction.North,
                    Health = lobby.Rules.PlayerMaxHealth,
                    PlayerId = p.UserId
                    
                };
                lobby.Players.Add(player);
            }

            LobbyGenerator.InitializeLobby(lobby);
            LobbyRepository repository = new LobbyRepository();
            repository.Create(lobby);

        }

        public static MessageConfig MoveCommand(int chatId, Direction direction, string username)
        {
            

            LobbyRepository repository = new LobbyRepository();
            Lobby lobby = repository.Read(repo.ReadLobbyId(chatId));
            var res = MazeLogic.TryMove(lobby, lobby.Players[lobby.CurrentTurn], direction);
            repository.Update(lobby);
            //TODO: удалить это говно
            FormatAnswers.ConsoleApp(lobby);
            if (res.Contains(PlayerAction.GameEnd))
            {
                lobby.IsActive = false;
                return new MessageConfig
                {
                    Answer = string.Format(Answers.EndGame.RandomAnswer(), username)
                };
            }

            MessageConfig msg = new MessageConfig
            {
                KeyBoardId = KeyBoardEnum.Move
            };

            List<string> ls = new List<string>();
            if (res.Contains(PlayerAction.OnWall))
            {
                msg.Answer = (string.Format(Answers.MoveWall.RandomAnswer(), username));
                return msg;
            }
            ls.Add(string.Format(Answers.MoveGo.RandomAnswer(), username));
            foreach (var item in res)
            {

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
            //TODO: реализовать нормально
            msg.Answer = string.Join("\n", ls);
            msg.AnswerForOther = msg.Answer;
            return msg;
        }
    }
}
