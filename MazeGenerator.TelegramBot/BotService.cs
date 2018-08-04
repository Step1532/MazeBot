using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using MazeGenerator.Core.GameGenerator;
using MazeGenerator.Core.Services;
using MazeGenerator.Core.Tools;
using MazeGenerator.Database;
using MazeGenerator.Models;
using MazeGenerator.Models.ActionStatus;
using MazeGenerator.Models.Enums;
using MazeGenerator.TelegramBot.Models;

namespace MazeGenerator.TelegramBot
{
    public static class BotService
    {
        public static readonly LobbyRepository LobbyRepository = new LobbyRepository();
        public static readonly MemberRepository MemberRepository = new MemberRepository();
        public static readonly CharacterRepository CharacterRepository = new CharacterRepository();

        public static List<MessageConfig> ShootCommand(int userId, Direction direction)
        {
            List<MessageConfig> msg = new List<MessageConfig>();
            var status = GameCommandService.ShootCommand(userId, direction);
            if (status.IsOtherTurn)
            {
                msg.Add(new MessageConfig
                {
                    Answer = string.Format(Answers.NoTurn.RandomAnswer()),
                    PlayerId = userId
                });
                return msg;
            }

            var username = status.CurrentPlayer.HeroName;
            var memberlist = MemberRepository.ReadMemberList(MemberRepository.ReadLobbyId(userId));
            if (status.Result == AttackType.NoAttack)
            {
                for (int i = 0; i < memberlist.Count; i++)
                {
                    if (memberlist[i].UserId == userId)
                    {
                        //TODO:
                        msg.Add(new MessageConfig
                        {
                            Answer = string.Format(Answers.NotBullet.RandomAnswer(), username),
                            PlayerId = memberlist[i].UserId
                        });
                    }
                    else
                    {
                        msg.Add(new MessageConfig
                        {
                            Answer = string.Format(Answers.NotBullet.RandomAnswer(), username),
                            PlayerId = memberlist[i].UserId
                        });
                    }
                }
                return msg;
            }


            var config = StatusToMessage.MessageOnShoot(status.Result, username);
            config.PlayerId = userId;
            msg.Add(config);
            if (status.ShootCount == false) config.KeyBoardId = KeybordConfiguration.WithoutShootKeyBoard();
            return msg;
        }

        public static List<MessageConfig> BombCommand(int userId, Direction direction)
        {
            List<MessageConfig> msg = new List<MessageConfig>();
            var status = GameCommandService.BombCommand(userId, direction);
            var memberlist = MemberRepository.ReadMemberList(MemberRepository.ReadLobbyId(userId));
            if (status.IsOtherTurn)
            {
                        msg.Add(new MessageConfig
                        {
                            Answer = string.Format(Answers.NoTurn.RandomAnswer()),
                            PlayerId = userId
                        });
                return msg;
            }
            var username = status.CurrentPlayer.HeroName;
            if (status.Result == BombResultType.Wall)
            {
                for (int i = 0; i < memberlist.Count; i++)
                {


                    if (memberlist[i].UserId == userId)
                    {
                        //TODO:
                        msg.Add(new MessageConfig
                        {
                            Answer = string.Format(Answers.ResultBombWall.RandomAnswer(), username),
                            PlayerId = memberlist[i].UserId
                        });
                    }
                    else
                    {
                        msg.Add(new MessageConfig
                        {
                            Answer = string.Format(Answers.ResultBombWall.RandomAnswer(), username),
                            PlayerId = memberlist[i].UserId
                        });
                    }
                }
            }
            else if (status.Result == BombResultType.NoBomb)
            {
                msg.Add(new MessageConfig
                {
                    Answer = string.Format(Answers.ResultBombNoBomb.RandomAnswer()),
                    PlayerId = userId
                });
                return msg;
            }
            else if (status.Result == BombResultType.Void)
            {
                for (int i = 0; i < memberlist.Count; i++)
                {
                    if (memberlist[i].UserId == userId)
                    {
                        //TODO:
                        msg.Add(new MessageConfig
                        {
                            Answer = string.Format(Answers.ResultBombVoid.RandomAnswer(), username),
                            PlayerId = memberlist[i].UserId
                        });
                    }
                    else
                    {
                        msg.Add(new MessageConfig
                        {
                            Answer = string.Format(Answers.ResultBombVoid.RandomAnswer(), username),
                            PlayerId = memberlist[i].UserId
                        });
                    }
                }
            }
            return msg;
        }

        public static List<MessageConfig> StabCommand(int userId)
        {
            //TODO: Назвать все userId и playerId в одном стиле
            var status = GameCommandService.StabCommand(userId);
            List<MessageConfig> msg = new List<MessageConfig>();
            if (status.IsOtherTurn)
            {
                msg.Add(new MessageConfig
                {
                    Answer = string.Format(Answers.NoTurn.RandomAnswer()),
                    PlayerId = userId
                });
                return msg;
            }

            var memberlist = MemberRepository.ReadMemberList(MemberRepository.ReadLobbyId(userId));
            var username = status.CurrentPlayer.HeroName;
            if (status.Result == AttackType.NoAttack)
            {
                for (int i = 0; i < memberlist.Count; i++)
                {
                    if (memberlist[i].UserId == userId)
                    {
                        //TODO:
                        msg.Add(new MessageConfig
                        {
                            Answer = string.Format(Answers.NotBullet.RandomAnswer(), username),
                            PlayerId = memberlist[i].UserId
                        });
                    }
                    else
                    {
                        msg.Add(new MessageConfig
                        {
                            Answer = string.Format(Answers.NotBullet.RandomAnswer(), username),
                            PlayerId = memberlist[i].UserId
                        });
                    }
                }

                return msg;
            }
            //return new MessageConfig
            //{
            //    TODO: сделать нормально
            //    TODO: NoBullet => OnEnemy
            //    Answer = string.Format(Answers.NotBullet.RandomAnswer(), username),
            //    AnswerForOther = string.Format(Answers.NotBullet.RandomAnswer(), username),
            //    OtherPlayersId = MemberRepository.ReadMemberList(MemberRepository.ReadLobbyId(userId))
            //        .Select(e => e.UserId)
            //        .ToList()
            //    NextPlayerId = lobby.Players[lobby.CurrentTurn].TelegramUserId,
            //    KeyBoardId = KeyboardType.Bomb
            //};

            var config = StatusToMessage.MessageOnStab(status.Result, username);
            config.PlayerId = userId;
            msg.Add(config);
            return msg;
        }

        public static List<MessageConfig> SkipTurn(int chatId)
        {
                    //TODO: написать тип SkipStatus
            var res = GameCommandService.SkipTurn(chatId);
            List<MessageConfig> msg = new List<MessageConfig>();
            if (res)
            {
                msg.Add(new MessageConfig
                {
                    Answer = string.Format(Answers.SkipTurn.RandomAnswer()),
                    PlayerId = chatId,
                });
                return msg;
            }

            msg.Add(new MessageConfig
            {
                Answer = string.Format(Answers.NoTurn.RandomAnswer()),
                PlayerId = chatId,
            });
            return msg;
        }

        public static List<MessageConfig> AfkCommand(int playerid)
        {
            var lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(playerid));
            var res = DateTime.Now.Subtract(lobby.TimeLastMsg);
            //TODO: засчитывать игроку бан
                List<MessageConfig> msg = new List<MessageConfig>();
            if (TimeSpan.Compare(lobby.Rules.BanTime, res) == -1)
            {
                lobby.IsActive = false;
                MemberRepository.Delete(lobby.GameId);
                msg.Add(new MessageConfig
                {
                    Answer = string.Format(Answers.AfkPlayer.RandomAnswer()),
                    PlayerId = playerid,
                });
                return msg;
            }
            msg.Add(new MessageConfig
            {
                Answer = "Дождитесь 24 часа после последнего сообщения",
                PlayerId = playerid,
            });
            return msg;
        }

        public static List<MessageConfig> MoveCommand(int chatId, Direction direction)
        {
            //TODO:
            var memberlist = MemberRepository.ReadMemberList(MemberRepository.ReadLobbyId(chatId));
            var status = GameCommandService.MoveCommand(chatId, direction);
            var username = status.CurrentPlayer?.HeroName;
            List<MessageConfig> msg = new List<MessageConfig>();
            if (status.IsOtherTurn)
            {
                msg.Add(new MessageConfig
                {
                    Answer = string.Format(Answers.NoTurn.RandomAnswer()),
                    PlayerId = chatId
                });
                return msg;
            }
            if (status.IsGameEnd)
            {
                
                for (int i = 0; i < memberlist.Count; i++)
                {
                    var character = CharacterRepository.ReadAll().Find(e => e.TelegramUserId == memberlist[i].UserId);
                        character.State = CharacterState.ChangeGameMode;
                    CharacterRepository.Update(character);
                    
                    if (memberlist[i].UserId == chatId)
                    {
                        //TODO:
                        msg.Add(new MessageConfig
                        {
                            Answer = string.Format(Answers.EndGame.RandomAnswer(), username),
                            PlayerId = memberlist[i].UserId
                        });
                    }
                    else
                    {
                        msg.Add(new MessageConfig
                        {
                            Answer = string.Format(Answers.EndGame.RandomAnswer(), username),
                            PlayerId = memberlist[i].UserId
                        });
                    }
                }

                return msg;
            }

            if (status.PlayerActions.Contains(PlayerAction.OnWall))
            {
                for (int i = 0; i < memberlist.Count; i++)
                {
                    if (memberlist[i].UserId == chatId)
                    {
                        //TODO:
                        msg.Add(new MessageConfig
                        {
                            Answer = string.Format(Answers.MoveWall.RandomAnswer(), username),
                            PlayerId = memberlist[i].UserId
                        });
                    }
                    else
                    {
                        msg.Add(new MessageConfig
                        {
                            Answer = string.Format(Answers.MoveWall.RandomAnswer(), username),
                            PlayerId = memberlist[i].UserId
                        });
                    }
                }

                return msg;
            }


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
            //return new MessageConfig
            //{
            //    KeyBoardId = KeyboardType.Move,
            //    Answer = string.Join("\n", messageList),
            //    AnswerForOther = string.Join("\n", messageList),
            //    CurrentPlayerId = chatId,
            //    OtherPlayersId = MemberRepository.ReadMemberList(MemberRepository.ReadLobbyId(chatId)).Where(e => e.UserId != chatId).Select(e => e.UserId).ToList(),

            //};
            msg.Add(new MessageConfig
            {
                Answer = string.Join("\n", messageList),
                PlayerId =  chatId
            });
            return msg;
        }

        public static List<MessageConfig> TryChangeName(string username, int playerId)
        {
            var loginRegex = new Regex("^[a-zA-Zа-яА-Я][a-zA-Zа-яА-Я0-9]{2,9}$");
            List<MessageConfig> msg = new List<MessageConfig>();
            if (loginRegex.Match(username).Success == false)
            {
                msg.Add(new MessageConfig
                {
                    PlayerId = playerId,
                    Answer = $"Используются неразрешенные символы"
                });
                return msg;
            }

            if (CharacterRepository.ReadAll().Any(e => e.CharacterName == username))
            {
                msg.Add(new MessageConfig
                {
                    PlayerId = playerId,
                    Answer = $"Имя существует"
                });
                return msg;
            }

            var r = CharacterRepository.Read(playerId);
            r.CharacterName = username;
            r.State = CharacterState.ChangeGameMode;
            CharacterRepository.Update(r);
            msg.Add(new MessageConfig
            {
                PlayerId = playerId,
                Answer = $"Имя _{username}_ задано"
            });
            return msg;
        }
    }
}