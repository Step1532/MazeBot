using System;
using System.Collections.Generic;
using System.Text;
using MazeGenerator.Core;
using MazeGenerator.Core.Services;
using MazeGenerator.Core.Tools;
using MazeGenerator.Database;
using MazeGenerator.Models;
using MazeGenerator.Models.ActionStatus;
using MazeGenerator.Models.Enums;
using MazeGenerator.TelegramBot.Models;

namespace MazeGenerator.TelegramBot
{
    class TutorialService
    {
        public static CharacterRepository CharacterRepository = new CharacterRepository();
        public static MemberRepository MemberRepository = new MemberRepository();
        public static LobbyRepository LobbyRepository = new LobbyRepository();

        public static MoveStatus MovCommand(int userId, Direction direction)
        {
            Lobby lobby = LobbyRepository.Read(0);
            var currentPlayer = lobby.Players.Find(e => e.TelegramUserId == userId);
            var actionList = PlayerLogic.TryMove(lobby, currentPlayer, direction);
            LobbyRepository.Update(lobby);
            FormatAnswers.ConsoleApp(lobby);
            if (actionList.Contains(PlayerAction.GameEnd))
            {
                return new MoveStatus
                {
                    IsGameEnd = true,
                    CurrentPlayer = currentPlayer,
                    PlayerActions = actionList
                };
            }

            MoveStatus status = new MoveStatus
            {
                IsOtherTurn = false,
                IsGameEnd = false,
                CurrentPlayer = currentPlayer,
                PlayerActions = actionList
            };
            return status;
        }

        public static List<MessageConfig> BomCommand(int userId, Direction direction)
        {
            List<MessageConfig> msg = new List<MessageConfig>();
            var status = BombCommand(userId, direction);
            var memberlist = MemberRepository.ReadMemberList(MemberRepository.ReadLobbyId(userId));
            if (status.IsOtherTurn)
            {
                msg.Add(new MessageConfig
                {
                    Answer = String.Format(Answers.NoTurn.RandomAnswer()),
                    PlayerId = userId
                });
                return msg;
            }
            var username = status.CurrentPlayer.HeroName;
            if (status.Result == BombResultType.Wall)
            {
                        msg.Add(new MessageConfig
                        {
                            Answer = String.Format(AnswersForOther.ResultBombWall.RandomAnswer(), username),
                            PlayerId = userId
                        });
               
            }
            else if (status.Result == BombResultType.NoBomb)
            {
                msg.Add(new MessageConfig
                {
                    Answer = String.Format(Answers.ResultBombNoBomb.RandomAnswer()),
                    PlayerId = userId
                });
                return msg;
            }
            else if (status.Result == BombResultType.Void)
            {

                msg.Add(new MessageConfig
                {
                    Answer = String.Format(AnswersForOther.ResultBombVoid.RandomAnswer(), username),
                    PlayerId =  userId
                });
            }

            return msg;
        }
        public static BombStatus BombCommand(int userId, Direction direction)
        {
            Lobby lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(userId));
            var currentPlayer = lobby.Players[lobby.CurrentTurn];

            var bombResult = PlayerLogic.Bomb(lobby, lobby.Players[lobby.CurrentTurn], direction);
            if (currentPlayer.Bombs == 0)
            {
                //TODO: correct keyboard
                //msg.KeyBoardId = KeyboardType.Bomb;
            }

            return new BombStatus
            {
                CurrentPlayer = currentPlayer,
                Result = bombResult
            };
        }

        public static List<MessageConfig> MoveCommand(int chatId, Direction direction)
        {
            var status = MovCommand(chatId, direction);
            var username = status.CurrentPlayer?.HeroName;
            List<MessageConfig> msg = new List<MessageConfig>();
            if (status.IsOtherTurn)
            {
                msg.Add(new MessageConfig
                {
                    Answer = String.Format(Answers.NoTurn.RandomAnswer()),
                    PlayerId = chatId
                });
                return msg;
            }
            if (status.IsGameEnd)
            {
                var character = CharacterRepository.Read(chatId);
                character.State = CharacterState.ChangeGameMode;
                msg.Add(new MessageConfig
                {
                    Answer = String.Format(Answers.EndGame.RandomAnswer(), username),
                    PlayerId = chatId
                });
                return msg;

            }
            if (status.PlayerActions.Contains(PlayerAction.OnWall))
            {
                msg.Add(new MessageConfig
                {
                    Answer = String.Format(Answers.MoveWall.RandomAnswer(), username),
                    PlayerId = chatId
                });
                return msg;
            }
            var messageList = new List<string>();
            messageList.Add(String.Format(Answers.MoveGo.RandomAnswer(), username));

            foreach (var item in status.PlayerActions)
            {
                var newString = StatusToMessage.MessageOnMoveAction(item, username);
                if (newString != null) messageList.Add(newString);
            }
            msg.Add(new MessageConfig
            {
                Answer = String.Join("\n", messageList),
                PlayerId = chatId
            });
            return msg;
        }
    }
}
