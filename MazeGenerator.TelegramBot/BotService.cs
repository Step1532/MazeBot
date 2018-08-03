using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Text.RegularExpressions;
using MazeGenerator.Core;
using MazeGenerator.Core.GameGenerator;
using MazeGenerator.Core.Services;
using MazeGenerator.Core.Tools;
using MazeGenerator.Database;
using MazeGenerator.Models;
using MazeGenerator.Models.Enums;
using MazeGenerator.TelegramBot.Models;
using Telegram.Bot.Args;
using Telegram.Bot.Requests;

namespace MazeGenerator.TelegramBot
{
    public static class BotService
    {
        public static readonly LobbyRepository LobbyRepository = new LobbyRepository();
        public static readonly MemberRepository MemberRepository = new MemberRepository();
        public static readonly CharacterRepository characters = new CharacterRepository();

        public static MessageConfig ShootCommand(int chatId, Direction direction, string username)
        {
            MessageConfig msg = new MessageConfig();
            if (CheckTurn(chatId))
            {
                return new MessageConfig
                {
                    Answer = string.Format(Answers.NoTurn.RandomAnswer()),
                    CurrentPlayerId = chatId
                };
            }
            Lobby lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(chatId));
            lobby.TimeLastMsg = DateTime.Now;

            var shootResult = PlayerLogic.TryShoot(lobby, lobby.Players[lobby.CurrentTurn], direction);

            LobbyRepository.Update(lobby);
           // var shootResult = GameCommandService.ShootCommand(chatId, direction);

            if (shootResult == null)
            {

                return new MessageConfig
                {
                    Answer = string.Format(Answers.NotBullet.RandomAnswer(), username),
                    AnswerForOther = string.Format(Answers.NotBullet.RandomAnswer(), username),
                    OtherPlayersId = MemberRepository.ReadMemberList(MemberRepository.ReadLobbyId(chatId))
                        .Select(e => e.UserId)
                        .ToList(),
                    NextPlayerId = lobby.Players[lobby.CurrentTurn].TelegramUserId,
                    KeyBoardId = KeyBoardEnum.Bomb
                };

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

                return msg;
            }
            else
            {
                msg.Answer = string.Format(Answers.ShootWall.RandomAnswer(), username);
                msg.AnswerForOther = string.Format(Answers.ShootWall.RandomAnswer(), username);
                if (shootResult.ShootCount == false)
                {
                    msg.KeyBoardId = KeyBoardEnum.Bomb;
                }
                return msg;
            }
        }

        public static MessageConfig BombCommand(int chatId, Direction direction)
        {
            Lobby lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(chatId));
            lobby.TimeLastMsg = DateTime.Now;
            var res = PlayerLogic.Bomb(lobby, lobby.Players[lobby.CurrentTurn], direction);
 
            MessageConfig msg = new MessageConfig();
            if (lobby.Players[lobby.CurrentTurn].Bombs == 0)
            {
                msg.KeyBoardId = KeyBoardEnum.Bomb;
            }

            var username = lobby.Players[lobby.CurrentTurn].HeroName;
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
        public static MessageConfig StabCommand(int playerId)
        {
            Lobby lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(playerId));
            lobby.TimeLastMsg = DateTime.Now;

            if (CheckTurn(playerId))
            {
                return new MessageConfig
                {
                    Answer = string.Format(Answers.NoTurn.RandomAnswer()),
                    CurrentPlayerId = playerId
                };
            }

            var stabResult = PlayerLogic.Stab(lobby, lobby.Players[lobby.CurrentTurn]);
            lobby.NextTurn();
            LobbyRepository.Update(lobby);

            if (stabResult.Player == null)
            {
                return new MessageConfig
                {
                    Answer = string.Format(Answers.ShootWall.RandomAnswer()),
                    AnswerForOther = string.Format(Answers.ShootWall.RandomAnswer()),
                    OtherPlayersId = MemberRepository.ReadMemberList(MemberRepository.ReadLobbyId(playerId))
                        .Select(e => e.UserId)
                        .ToList(),
                    NextPlayerId = lobby.Players[lobby.CurrentTurn].TelegramUserId
                };
            }

            if (stabResult.Result == ResultShoot.Hit)
            {
                return new MessageConfig
                {
                    Answer = string.Format(Answers.ShootHit.RandomAnswer(), stabResult.Player.HeroName),
                    AnswerForOther = string.Format(Answers.ShootHit.RandomAnswer(), stabResult.Player.HeroName),
                    OtherPlayersId = MemberRepository.ReadMemberList(MemberRepository.ReadLobbyId(playerId))
                        .Select(e => e.UserId)
                        .ToList(),
                    NextPlayerId = lobby.Players[lobby.CurrentTurn].TelegramUserId
                };
            }

            return new MessageConfig
            {
                Answer = string.Format(Answers.ShootKill.RandomAnswer(), stabResult.Player.HeroName),
                AnswerForOther = string.Format(Answers.ShootKill.RandomAnswer(), stabResult.Player.HeroName),
                OtherPlayersId = MemberRepository.ReadMemberList(MemberRepository.ReadLobbyId(playerId))
                    .Select(e => e.UserId)
                    .ToList(),
                NextPlayerId = lobby.Players[lobby.CurrentTurn].TelegramUserId
            };


        }

        public static MessageConfig SkipTurn(int chatId)
        {
            Lobby lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(chatId));
            lobby.TimeLastMsg = DateTime.Now;
            if (lobby.Players.FindIndex(e => e.TelegramUserId == chatId) != lobby.CurrentTurn)
            {
                return new MessageConfig
                {
                    Answer = string.Format(Answers.NoTurn.RandomAnswer())
                };
            }

            var res = lobby.Players[lobby.CurrentTurn];
            lobby.CurrentTurn++;
            if (lobby.CurrentTurn == lobby.Players.Count)
                lobby.CurrentTurn = 0;

                return new MessageConfig
                {
                    Answer = string.Format(Answers.SkipTurn.RandomAnswer(), res.HeroName),
                    AnswerForOther = null,
                };
        }
        public static MessageConfig AfkCommand(int playerid)
        {
            Lobby lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(playerid));
            var res = DateTime.Now.Subtract(lobby.TimeLastMsg);
            //TODO: засчитывать игроку бан
            if (TimeSpan.Compare(lobby.Rules.BanTime, res) == -1)
            {
                lobby.IsActive = false;
                MemberRepository.Delete(lobby.GameId);
                return new MessageConfig
                {
                    Answer = string.Format(Answers.AfkPlayer.RandomAnswer())
                };
            }
            else
            {
                return new MessageConfig
                {
                    Answer = "Дождитесь 24 часа после последнего сообщения"
                };
            }


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

        public static MessageConfig MoveCommand(int chatId, Direction direction)
        {
            Lobby lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(chatId));
            lobby.TimeLastMsg = DateTime.Now;
            if (CheckTurn(chatId))
            {
                return new MessageConfig
                {
                    Answer = string.Format(Answers.NoTurn.RandomAnswer())
                };
            }

            var res = PlayerLogic.TryMove(lobby, lobby.Players[lobby.CurrentTurn], direction);
            Player currentPlayer = lobby.Players[lobby.CurrentTurn];
            var username = currentPlayer.HeroName;
            lobby.CurrentTurn++;
            if (lobby.CurrentTurn == lobby.Players.Count)
                lobby.CurrentTurn = 0;

            LobbyRepository.Update(lobby);
            //TODO: удалить это говно
            FormatAnswers.ConsoleApp(lobby);
            if (res.Contains(PlayerAction.GameEnd))
            {
                lobby.IsActive = false;
                MemberRepository.Delete(lobby.GameId);
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
                var playersOnCell = MazeLogic.PlayersOnCell(currentPlayer, lobby);
                foreach (var e in playersOnCell)
                {
                    ls.Add(string.Format(Answers.MovePlayer.RandomAnswer(), username, e.HeroName));
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

        private static bool CheckTurn(int chatId)
        {
            Lobby lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(chatId));
            if (lobby.Players.FindIndex(e => e.TelegramUserId == chatId) != lobby.CurrentTurn)
            {
                return true;
            }

            return false;
        }

        public static MessageConfig TryChangeName(string username, int playerId)
        {
            Regex login_regex = new Regex("^[a-zA-Zа-яА-Я][a-zA-Zа-яА-Я0-9]{2,9}$");
            if (login_regex.Match(username).Success == false)
            {
                return new MessageConfig()
                {
                    CurrentPlayerId = playerId,
                    Answer = $"Используются неразрешенные символы"
                };
            }

            if (characters.ReadAll().Any(e => e.CharacterName == username))
            {
                return new MessageConfig()
                {
                    CurrentPlayerId = playerId,
                    Answer = $"Имя существует"
                };
            }

            var r = characters.Read(playerId);
            r.CharacterName = username;
            r.State = CharacterState.ChangeGameMode;
            characters.Update(r);

            return new MessageConfig()
            {
                CurrentPlayerId = playerId,
                Answer = $"Имя _{username}_задано"
            };
        }
    }
}
