using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MazeGenerator.Core.GameGenerator;
using MazeGenerator.Core.Services;
using MazeGenerator.Core.Tools;
using MazeGenerator.Database;
using MazeGenerator.Models;
using MazeGenerator.Models.ActionStatus;
using MazeGenerator.Models.Enums;

namespace MazeGenerator.TelegramBot
{
    public static class BotService
    {
        public static readonly LobbyRepository LobbyRepository = new LobbyRepository();
        public static readonly MemberRepository MemberRepository = new MemberRepository();
        public static readonly CharacterRepository CharacterRepository = new CharacterRepository();

        public static MessageConfig ShootCommand(int userId, Direction direction)
        {
            var status = GameCommandService.ShootCommand(userId, direction);
            if (status.IsOtherTurn)
                return new MessageConfig
                {
                    Answer = string.Format(Answers.NoTurn.RandomAnswer()),
                    CurrentPlayerId = userId
                };

            var username = status.CurrentPlayer.HeroName;
            if (status.Result == AttackType.NoAttack)
                return new MessageConfig
                {
                    //TODO: сделать нормально
                    Answer = string.Format(Answers.NotBullet.RandomAnswer(), username),
                    AnswerForOther = string.Format(Answers.NotBullet.RandomAnswer(), username),
                    OtherPlayersId = MemberRepository.ReadMemberList(MemberRepository.ReadLobbyId(userId))
                        .Select(e => e.UserId)
                        .ToList()
                    //NextPlayerId = lobby.Players[lobby.CurrentTurn].TelegramUserId,
                    //KeyBoardId = KeyboardType.Bomb
                };

            var config = StatusToMessage.MessageOnShoot(status.Result, username);

            //TODO: ??
            if (status.ShootCount == false) config.KeyBoardId = KeyboardType.Bomb;
            return config;
        }

        public static MessageConfig BombCommand(int userId, Direction direction)
        {
            var status = GameCommandService.BombCommand(userId, direction);
            if (status.IsOtherTurn)
                return new MessageConfig
                {
                    Answer = string.Format(Answers.NoTurn.RandomAnswer()),
                    CurrentPlayerId = userId
                };

            var username = status.CurrentPlayer.HeroName;
            var msg = new MessageConfig
            {
                CurrentPlayerId = userId
            };
            switch (status.Result)
            {
                case BombResultType.Wall:
                    msg.Answer = string.Format(Answers.ResultBombWall.RandomAnswer(), username);
                    break;
                case BombResultType.NoBomb:
                    msg.Answer = string.Format(Answers.ResultBombNoBomb.RandomAnswer(), username);
                    break;
                case BombResultType.Void:
                    msg.Answer = string.Format(Answers.ResultBombVoid.RandomAnswer(), username);
                    break;
            }

            return msg;
        }

        public static MessageConfig StabCommand(int playerId)
        {
            //TODO: Назвать все userId и playerId в одном стиле
            var status = GameCommandService.StabCommand(playerId);

            if (status.IsOtherTurn)
                return new MessageConfig
                {
                    Answer = string.Format(Answers.NoTurn.RandomAnswer()),
                    CurrentPlayerId = playerId
                };

            var username = status.CurrentPlayer.HeroName;
            if (status.Result == AttackType.NoAttack)
                return new MessageConfig
                {
                    //TODO: сделать нормально
                    //TODO: NoBullet => OnEnemy
                    Answer = string.Format(Answers.NotBullet.RandomAnswer(), username),
                    AnswerForOther = string.Format(Answers.NotBullet.RandomAnswer(), username),
                    OtherPlayersId = MemberRepository.ReadMemberList(MemberRepository.ReadLobbyId(playerId))
                        .Select(e => e.UserId)
                        .ToList()
                    //NextPlayerId = lobby.Players[lobby.CurrentTurn].TelegramUserId,
                    //KeyBoardId = KeyboardType.Bomb
                };

            var config = StatusToMessage.MessageOnStab(status.Result, username);
            return config;
        }

        public static MessageConfig SkipTurn(int chatId)
        {
            var res = GameCommandService.SkipTurn(chatId);
            if (res)
                return new MessageConfig
                {
                    //TODO: написать тип SkipStatus
                    //Answer = string.Format(Answers.SkipTurn.RandomAnswer(), res.HeroName),
                    AnswerForOther = null
                };
            return new MessageConfig
            {
                Answer = string.Format(Answers.NoTurn.RandomAnswer())
            };
        }

        public static MessageConfig AfkCommand(int playerid)
        {
            var lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(playerid));
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

            return new MessageConfig
            {
                Answer = "Дождитесь 24 часа после последнего сообщения"
            };
        }

        public static void StartGame(int playerId)
        {
            var repo = new MemberRepository();
            var gameid = repo.ReadLobbyId(playerId);
            var players = repo.ReadMemberList(gameid);
            var lobby = new Lobby(gameid);
            foreach (var p in players)
            {
                var player = new Player
                {
                    Rotate = Direction.North,
                    Health = lobby.Rules.PlayerMaxHealth,
                    TelegramUserId = p.UserId
                };
                lobby.Players.Add(player);
            }

            LobbyGenerator.InitializeLobby(lobby);
            var repository = new LobbyRepository();
            repository.Create(lobby);
        }

        public static MessageConfig MoveCommand(int chatId, Direction direction)
        {
            var status = GameCommandService.MoveCommand(chatId, direction);
            var username = status.CurrentPlayer?.HeroName;

            if (status.IsOtherTurn)
                return new MessageConfig
                {
                    Answer = string.Format(Answers.NoTurn.RandomAnswer())
                };

            if (status.IsGameEnd)
                return new MessageConfig
                {
                    Answer = string.Format(Answers.EndGame.RandomAnswer(), username)
                };

            if (status.PlayerActions.Contains(PlayerAction.OnWall))
                return new MessageConfig
                {
                    KeyBoardId = KeyboardType.Move,
                    Answer = string.Format(Answers.MoveWall.RandomAnswer(), username)
                };


            var messageList = new List<string>();
            messageList.Add(string.Format(Answers.MoveGo.RandomAnswer(), username));

            if (status.PlayersOnSameCell != null)
                foreach (var player in status.PlayersOnSameCell)
                    messageList.Add(string.Format(Answers.MovePlayer.RandomAnswer(), username, player.HeroName));

            foreach (var item in status.PlayerActions)
            {
                var newString = StatusToMessage.MessageOnMoveAction(item, username);
                if (newString != null) messageList.Add(newString);
            }

            //TODO: реализовать нормально
            return new MessageConfig
            {
                KeyBoardId = KeyboardType.Move,
                Answer = string.Join("\n", messageList),
                AnswerForOther = string.Join("\n", messageList)
            };
        }

        public static MessageConfig TryChangeName(string username, int playerId)
        {
            var loginRegex = new Regex("^[a-zA-Zа-яА-Я][a-zA-Zа-яА-Я0-9]{2,9}$");
            if (loginRegex.Match(username).Success == false)
                return new MessageConfig
                {
                    CurrentPlayerId = playerId,
                    Answer = $"Используются неразрешенные символы"
                };

            if (CharacterRepository.ReadAll().Any(e => e.CharacterName == username))
                return new MessageConfig
                {
                    CurrentPlayerId = playerId,
                    Answer = $"Имя существует"
                };

            var r = CharacterRepository.Read(playerId);
            r.CharacterName = username;
            r.State = CharacterState.ChangeGameMode;
            CharacterRepository.Update(r);

            return new MessageConfig
            {
                CurrentPlayerId = playerId,
                Answer = $"Имя _{username}_задано"
            };
        }
    }
}