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
using Telegram.Bot.Requests;

namespace MazeGenerator.TelegramBot
{
    public static class BotService
    {
        public static readonly LobbyRepository LobbyRepository = new LobbyRepository();
        public static readonly MemberRepository MemberRepository = new MemberRepository();

        public static MessageConfig ShootCommand(int chatId, Direction direction, string username)
        {
            Lobby lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(chatId));
            var shootResult = MazeLogic.TryShoot(lobby, lobby.Players[lobby.CurrentTurn], direction);
            //TODO
            LobbyRepository.Update(lobby);

            if (shootResult == null)
            {
                return new MessageConfig
                {
                    Answer = string.Format(Answers.NotBullet.RandomAnswer(), username),
                    AnswerForOther = string.Format(Answers.NotBullet.RandomAnswer(), username),
                    KeyBoardId = KeyBoardEnum.Bomb
                };
            }

            if (shootResult.Player != null)
            {
                var msg = new MessageConfig();

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

                return msg;
            }
            else
            {
                MessageConfig msg = new MessageConfig();

                msg.Answer = string.Format(Answers.ShootWall.RandomAnswer(), username);
                msg.AnswerForOther = string.Format(Answers.ShootWall.RandomAnswer(), username);
                if (shootResult.ShootCount == false)
                {
                    msg.KeyBoardId = KeyBoardEnum.Bomb;
                }
                return msg;
            }
        }

        public static MessageConfig BombCommand(int chatId, Direction direction, string username)
        {
            Lobby lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(chatId));
            var res = MazeLogic.Bomb(lobby, lobby.Players[lobby.CurrentTurn], direction);
 
            MessageConfig msg = new MessageConfig();
            if (lobby.Players[lobby.CurrentTurn].Bombs == 0)
            {
                msg.KeyBoardId = KeyBoardEnum.Bomb;
            }

            switch (res)
            {
                case ResultBomb.Wall:
                    msg.Answer =  string.Format(Answers.ResultBombWall.RandomAnswer(), username);
                    break;
                case ResultBomb.NoBomb:
                    msg.Answer = string.Format(Answers.ResultBombNoBomb.RandomAnswer(), username);
                    break;
                case ResultBomb.Void:
                    msg.Answer =  string.Format(Answers.ResultBombVoid.RandomAnswer(), username);
                    break;
            }

            lobby.CurrentTurn++;
            if (lobby.CurrentTurn == lobby.Players.Count)
                lobby.CurrentTurn = 0;

            LobbyRepository.Update(lobby);
            return msg;
        }
        public static MessageConfig StabCommand(int chatId)
        {
            Lobby lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(chatId));
            if (lobby.Players.FindIndex(e => e.TelegramUserId == chatId) != lobby.CurrentTurn)
            {
                return new MessageConfig
                {
                    Answer = string.Format(Answers.NoTurn.RandomAnswer())
                };
            }

            var res = MazeLogic.Stab(lobby, lobby.Players[lobby.CurrentTurn]);
            lobby.CurrentTurn++;
            if (lobby.CurrentTurn == lobby.Players.Count)
                lobby.CurrentTurn = 0;
            LobbyRepository.Update(lobby);

            if (res != null)
            {
                return new MessageConfig
                {
                    Answer = string.Format(Answers.ShootHit.RandomAnswer(), res.HeroName),
                    AnswerForOther = null,
                };
            }
            return new MessageConfig
            {
                Answer = string.Format(Answers.ShootWall.RandomAnswer()),
                AnswerForOther = null,
            };

            throw new Exception();
        }
        public static MessageConfig SkipTurn(int chatId)
        {
            Lobby lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(chatId));
            if (lobby.Players.FindIndex(e => e.TelegramUserId == chatId) != lobby.CurrentTurn)
            {
                return new MessageConfig
                {
                    Answer = string.Format(Answers.NoTurn.RandomAnswer())
                };
            }

            var res = lobby.Players[lobby.CurrentTurn];
            lobby.CurrentTurn++;
            if (lobby.CurrentTurn == 2)
                lobby.CurrentTurn = 0;
                return new MessageConfig
                {
                    Answer = string.Format(Answers.ShootHit.RandomAnswer(), res.HeroName),
                    AnswerForOther = null,
                };
        }

        public static void StartGame(int playerId)
        {
            MemberRepository repo = new MemberRepository();
            var gameid = repo.ReadLobbyId(playerId);
            var players = repo.ReadMemberList(gameid);
            Lobby lobby = new Lobby(gameid);
            foreach (var p in players)
            {
                Player player = new Player
                {
                    Rotate = Direction.North,
                    Health = lobby.Rules.PlayerMaxHealth,
                    TelegramUserId = p.UserId,
                };
                lobby.Players.Add(player);
            }

            LobbyGenerator.InitializeLobby(lobby);
            LobbyRepository repository = new LobbyRepository();
            repository.Create(lobby);

        }

        public static MessageConfig MoveCommand(int chatId, Direction direction, string username)
        {

            Lobby lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(chatId));
            if (lobby.Players.FindIndex(e => e.TelegramUserId == chatId) != lobby.CurrentTurn)
            {
                return new MessageConfig
                {
                    Answer = string.Format(Answers.NoTurn.RandomAnswer(), username)
                };
            }

            var res = MazeLogic.TryMove(lobby, lobby.Players[lobby.CurrentTurn], direction);
            Player currentPlayer = lobby.Players[lobby.CurrentTurn];
            lobby.CurrentTurn++;
            if (lobby.CurrentTurn == lobby.Players.Count)
                lobby.CurrentTurn = 0;

            LobbyRepository.Update(lobby);
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

            if (res.Contains(PlayerAction.MeetPlayer))
            {
                //TODO:
                var playersOnCell = LobbyService.PlayersOnCell(currentPlayer, lobby);
               //TODO: переделать под cHARACTER
                foreach (var e in playersOnCell)
                {
                    GetChatMemberRequest a = new GetChatMemberRequest(e.TelegramUserId, e.TelegramUserId);
                    ls.Add(string.Format(Answers.MovePlayer.RandomAnswer(), username, a.ChatId.Username));
                }
            }

            foreach (var item in res)
            {

                if (item == PlayerAction.FakeChest)
                {
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
