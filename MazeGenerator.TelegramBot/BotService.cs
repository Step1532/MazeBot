using System;
using System.Collections.Generic;
using System.IO;
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
using Telegram.Bot.Requests;

namespace MazeGenerator.TelegramBot
{
    //TODO: отрефакторить это все
    public static class BotService
    {
        public static readonly LobbyRepository LobbyRepository = new LobbyRepository();
        public static readonly MemberRepository MemberRepository = new MemberRepository();
        public static readonly CharacterRepository CharacterRepository = new CharacterRepository();

        public static List<MessageConfig> ShootCommand(int userId, Direction direction)
        {
            List<MessageConfig> msg = new List<MessageConfig>();
            var status = GameCommandService.ShootCommand(userId, direction);
            //TODO: вынести в отдебный метод отправку "не ваш ход"
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
            var memberlist = MemberRepository.ReadMemberList(MemberRepository.ReadLobbyId(userId));
            var lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(userId));
            var nextPlayer = lobby.Players[lobby.CurrentTurn];
            if (status.Result == AttackType.NoAttack)
            {
                for (int i = 0; i < memberlist.Count; i++)
                {
                    if (memberlist[i].UserId == userId)
                    {
                        msg.Add(new MessageConfig
                        {
                            Answer = String.Format(Answers.NotBullet.RandomAnswer(), username),
                            PlayerId = memberlist[i].UserId
                        });
                    }
                    else
                    {
                        msg.Add(new MessageConfig
                        {
                            Answer = String.Format(AnswersForOther.NotBullet.RandomAnswer(), username),
                            PlayerId = memberlist[i].UserId
                        });
                    }
                }
                msg.Find(e => e.PlayerId == nextPlayer.TelegramUserId).KeyBoardId = GetKeyboardType(nextPlayer);
                return msg;
            }

            if (status.Result == AttackType.NoTarget)
            {
                    msg.Add(new MessageConfig
                    {
                        Answer = String.Format(Answers.ShootWall.RandomAnswer(), username,
                            Extensions.DirectionToString(direction)),
                        PlayerId = userId,
                        KeyBoardId = status.KeyboardType
                    });

                msg.AddRange(memberlist
                    .Where(m => m.UserId != userId)
                    .Select(m => new MessageConfig
                    {
                        Answer = String.Format(AnswersForOther.ShootWall.RandomAnswer(), username, Extensions.DirectionToString(direction)),
                        PlayerId = m.UserId
                    }));
                msg.Find(e => e.PlayerId == nextPlayer.TelegramUserId).KeyBoardId = GetKeyboardType(nextPlayer);
                return msg;
            }
            var config = StatusToMessage.MessageOnShoot(status.Result, username, status.Target.HeroName,
                direction.DirectionToString());
                msg.Add(new MessageConfig
                {
                    Answer = config.Item1,
                    PlayerId = userId
                });

                msg.AddRange(memberlist
                    .Where(m => m.UserId != userId)
                    .Select(m => new MessageConfig
                    {
                        Answer = config.Item2,
                        PlayerId = m.UserId
                    }));


            //if (status.ShootCount == false) 
            msg.Find(e => e.PlayerId == userId).KeyBoardId = status.KeyboardType;
                msg.Find(e => e.PlayerId == nextPlayer.TelegramUserId).KeyBoardId = GetKeyboardType(nextPlayer);
            return msg;
        }

        public static List<MessageConfig> BombCommand(int userId, Direction direction)
        {
            List<MessageConfig> msg = new List<MessageConfig>();
            var memberlist = MemberRepository.ReadMemberList(MemberRepository.ReadLobbyId(userId));
            var status = GameCommandService.BombCommand(userId, direction);
            if (status.IsOtherTurn)
            {
                msg.Add(new MessageConfig
                {
                    Answer = String.Format(Answers.NoTurn.RandomAnswer()),
                    PlayerId = userId
                });
                return msg;
            }

            var lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(userId));
            var nextPlayer = lobby.Players[lobby.CurrentTurn];
            var username = status.CurrentPlayer.HeroName;
            if (status.Result == BombResultType.Wall)
            {
                msg.Add(new MessageConfig
                {
                    Answer = String.Format(Answers.ResultBombWall.RandomAnswer(), username) + '\n',
                    PlayerId = userId
                });

                msg.AddRange(memberlist
                    .Where(m => m.UserId != userId)
                    .Select(m => new MessageConfig
                    {
                        Answer = String.Format(AnswersForOther.ResultBombWall.RandomAnswer(), username, Extensions.DirectionToString(direction)),
                        PlayerId = m.UserId
                    }));
            }
            else if (status.Result == BombResultType.NoBomb)
            {
                msg.Add(new MessageConfig
                {
                    Answer = String.Format(Answers.ResultBombNoBomb.RandomAnswer()),
                    PlayerId = userId
                });
            }
            else if (status.Result == BombResultType.Void)
            {
                for (int i = 0; i < memberlist.Count; i++)
                {
                    msg.Add(new MessageConfig
                    {
                        Answer = String.Format(Answers.ResultBombVoid.RandomAnswer(), username) + '\n',
                        PlayerId = userId
                    });

                    msg.AddRange(memberlist
                        .Where(m => m.UserId != userId)
                        .Select(m => new MessageConfig
                        {
                            Answer = String.Format(AnswersForOther.ResultBombVoid.RandomAnswer(), username,
                                direction.DirectionToString()),
                            PlayerId = m.UserId
                        }));
                }

                
            }
            //            if (status.BombCount == false) 
            msg.Find(e => e.PlayerId == userId).KeyBoardId = status.KeyboardId;

                msg.Find(e => e.PlayerId == nextPlayer.TelegramUserId).KeyBoardId = GetKeyboardType(nextPlayer);
            return msg;

        }

        public static List<MessageConfig> StabCommand(int userId)
        {
            //TODO: Назвать все userId и playerId в одном стиле
            var status = GameCommandService.StabCommand(userId);
            List<MessageConfig> msg = new List<MessageConfig>();
            var lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(userId));
            var nextPlayer = lobby.Players[lobby.CurrentTurn];
            if (status.IsOtherTurn)
            {
                msg.Add(new MessageConfig
                {
                    Answer = String.Format(Answers.NoTurn.RandomAnswer()),
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
                        msg.Add(new MessageConfig
                        {
                            Answer = String.Format(Answers.StabWall.RandomAnswer(), username),
                            PlayerId = memberlist[i].UserId
                        });
                    }
                    else
                    {
                        msg.Add(new MessageConfig
                        {
                            Answer = String.Format(AnswersForOther.StabWall.RandomAnswer(), username),
                            PlayerId = memberlist[i].UserId
                        });
                    }
                }

                msg.Find(e => e.PlayerId == nextPlayer.TelegramUserId).KeyBoardId = GetKeyboardType(nextPlayer);
                return msg;
            }


            var config = StatusToMessage.MessageOnStab(status.Result, username, status.Target.HeroName);
            msg.Add(new MessageConfig
            {
                Answer = config.Item1,
                PlayerId = userId
            });

            msg.AddRange(memberlist
                .Where(m => m.UserId != userId)
                .Select(m => new MessageConfig
                {
                    Answer = config.Item2,
                    PlayerId = m.UserId
                }));
            return msg;
        }

        public static List<MessageConfig> SkipTurn(int chatId)
        {
                    //TODO: написать тип SkipStatus
            CharacterRepository character = new CharacterRepository();
            var res = GameCommandService.SkipTurn(chatId);
            var memberlist = MemberRepository.ReadMemberList(MemberRepository.ReadLobbyId(chatId));
            var username = character.Read(chatId).CharacterName;

            List<MessageConfig> msg = new List<MessageConfig>();
            //TODO
            var lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(chatId));
            var nextPlayer = lobby.Players[lobby.CurrentTurn];
            if (res.CanMakeTurn)
            {
                if (res.PickChest)
                {
                    msg.Add(new MessageConfig
                    {
                        Answer = String.Format(Answers.SkipTurn.RandomAnswer(), username) + '\n' + String.Format(Answers.MoveChest.RandomAnswer(), username),
                        PlayerId = chatId
                    });

                    msg.AddRange(memberlist
                        .Where(m => m.UserId != chatId)
                        .Select(m => new MessageConfig
                        {
                            Answer = String.Format(AnswersForOther.SkipTurn.RandomAnswer(), username) + '\n' + String.Format(AnswersForOther.MoveChest.RandomAnswer(), username),
                            PlayerId = m.UserId
                        }));
                }
                else
                {
                    msg.Add(new MessageConfig
                    {
                        Answer = String.Format(Answers.SkipTurn.RandomAnswer(), username) + '\n',
                        PlayerId = chatId
                    });

                    msg.AddRange(memberlist
                        .Where(m => m.UserId != chatId)
                        .Select(m => new MessageConfig
                        {
                            Answer = String.Format(AnswersForOther.SkipTurn.RandomAnswer(), username),
                            PlayerId = m.UserId
                        }));
                }
                msg.Find(e => e.PlayerId == nextPlayer.TelegramUserId).KeyBoardId = GetKeyboardType(nextPlayer);
                return msg;
            }
            msg.Add(new MessageConfig
            {
                Answer = String.Format(Answers.NoTurn.RandomAnswer()),
                PlayerId = chatId,
            });
            return msg;
        }

        //TODO: переписать
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
                    Answer = String.Format(Answers.AfkPlayer.RandomAnswer()),
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
        //todo
        public static List<MessageConfig> LeaveCommand(int playerid)
        {
            List<MessageConfig> msg = new List<MessageConfig>();
            var lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(playerid));
            var ls = lobby.Players.Select(e => e.TelegramUserId).ToList();
            foreach (var item in ls)
            {
                var hero = CharacterRepository.Read(item);
                hero.State = CharacterState.ChangeGameMode;
                CharacterRepository.Update(hero);
                msg.Add(new MessageConfig
                {
                    //todo answer
                    Answer = String.Format(Answers.AfkPlayer.RandomAnswer()),
                    PlayerId = playerid,
                });
            }
            var res = DateTime.Now;
            var c = CharacterRepository.Read(playerid);
            c.State = CharacterState.Ban;
            c.BanTime = res;
            CharacterRepository.Update(c);
            lobby.IsActive = false;
            MemberRepository.Delete(lobby.GameId);
            msg.Add(new MessageConfig
            {
                Answer = "Вы забанены",
                PlayerId = playerid,
            });
            return msg;
        }
        public static List<MessageConfig> TryLeaveCommand(int playerid)
        {
            List<MessageConfig> msg = new List<MessageConfig>();
            var c = CharacterRepository.Read(playerid);
            c.State = CharacterState.AcceptLeave;
            CharacterRepository.Update(c);
            msg.Add(new MessageConfig
            {
                Answer = "Напишите подтверждение - \"Подтверждаю\"",
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
                    Answer = String.Format(Answers.NoTurn.RandomAnswer()),
                    PlayerId = chatId
                });
                return msg;
            }

            if (status.IsGameEnd)
            {
                //TODO: рефактринг, заменить for на AddRabge и LINQ
                foreach (var member in memberlist)
                {
                    var character = CharacterRepository
                        .ReadAll().Find(e => e.TelegramUserId == member.UserId);
                    character.State = CharacterState.ChangeGameMode;
                    CharacterRepository.Update(character);
                    
                    if (member.UserId == chatId)
                    {
                        msg.Add(new MessageConfig
                        {
                            Answer = String.Format(Answers.EndGame.RandomAnswer(), username),
                            PlayerId = member.UserId
                        });
                    }
                    else
                    {
                        msg.Add(new MessageConfig
                        {
                            Answer = String.Format(AnswersForOther.EndGame.RandomAnswer(), username, Extensions.DirectionToString(direction)),
                            PlayerId = member.UserId
                        });
                    }
                }
                return msg;
            }

            var lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(chatId));
            var nextPlayer = lobby.Players[lobby.CurrentTurn];
            if (status.PlayerActions.Contains(PlayerAction.OnWall))
            {
                msg.Add(new MessageConfig
                {
                    Answer = String.Format(Answers.MoveWall.RandomAnswer(), username) + '\n',
                    PlayerId = chatId
                });

                msg.AddRange(memberlist
                    .Where(m => m.UserId != chatId)
                    .Select(m => new MessageConfig
                    {
                        Answer = String.Format(AnswersForOther.MoveWall.RandomAnswer(), username, Extensions.DirectionToString(direction)),
                        PlayerId = m.UserId
                    }));
                msg.Find(e => e.PlayerId == nextPlayer.TelegramUserId).KeyBoardId = GetKeyboardType(nextPlayer);
                return msg;
            }

            string s = "";
            var messageList = new List<string>();

            foreach (var item in status.PlayerActions)
            {
                var newString = StatusToMessage.MessageOnMoveAction(item, username);
                if (newString != null)
                    messageList.Add(newString);
            }


                 //   messageList.Add(String.Format(Answers.MovePlayer.RandomAnswer(), username, player.HeroName));

            s = string.Join("\\n", messageList);
            if(status.PlayerActions.Contains(PlayerAction.OnArsenal))
            msg.Add(new MessageConfig
            {
                Answer = String.Format(Answers.MoveGo.RandomAnswer(), username) + '\n' + s,
                PlayerId = chatId,
                KeyBoardId  = KeyboardType.ShootwithBomb
            });
            else
            {
                msg.Add(new MessageConfig
                {
                    Answer = String.Format(Answers.MoveGo.RandomAnswer(), username) + '\n' + s,
                    PlayerId = chatId,
                });
            }

            msg.AddRange(memberlist
                .Where(m => m.UserId != chatId)
                .Select(m => new MessageConfig
                {
                    Answer = String.Format(AnswersForOther.MoveGo.RandomAnswer(), username, Extensions.DirectionToString(direction)) + s,
                    PlayerId = m.UserId
                }));
            if (status.PlayersOnSameCell != null)
            {
                string answ1 = "", answ2 = "";
                foreach (var player in status.PlayersOnSameCell)
                {
                    answ1 += String.Format(Answers.MovePlayer.RandomAnswer(), player.HeroName) + '\n';
                    answ2 += String.Format(AnswersForOther.MovePlayer.RandomAnswer(), username, player.HeroName,
                        Extensions.DirectionToString(direction));

                }
                msg.Add(new MessageConfig
                {
                    Answer = answ1,
                    PlayerId = chatId
                });

                msg.AddRange(memberlist
                    .Where(m => m.UserId != chatId)
                    .Select(m => new MessageConfig
                    {
                        Answer = answ2,
                        PlayerId = m.UserId
                    }));
            }
                msg.Find(e => e.PlayerId == nextPlayer.TelegramUserId).KeyBoardId = GetKeyboardType(nextPlayer);
            return msg;
        }

        public static List<MessageConfig> TryChangeName(string username, int playerId)
        {
            var loginRegex = new Regex("^[a-zA-Zа-яА-Я][a-zA-Zа-яА-Я0-9]{3,15}$");
            List<MessageConfig> msg = new List<MessageConfig>();
            if (loginRegex.Match(username).Success == false)
            {
                msg.Add(new MessageConfig
                {
                    PlayerId = playerId,
                    Answer = $"Имя персонажа может содержать буквы *A-Z*, *А-Я*, английского и русского алфавита, а также *цифры*. Длинна ника не должно превышать *15* символов, и не должна быть короче *3-x*"
                });
                return msg;
            }

            if (CharacterRepository.ReadAll().Any(e => e.CharacterName == username))
            {
                msg.Add(new MessageConfig
                {
                    PlayerId = playerId,
                    Answer = $"Такой ник уже существует"
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
                Answer = $"Имя *{username}* задано, нажми /game для поиска игры"
            });
            return msg;
        }

        public static List<MessageConfig> FindGameCommand(int playerId)
        {
            var members = new MemberRepository();
            var characterRepository = new CharacterRepository();
            var lobyRepository = new LobbyRepository();
            var msg = new List<MessageConfig>();
            if (LobbyService.CheckLobby(playerId))
            {
                msg.Add(new MessageConfig
                {
                    Answer = "Вы уже находитесь в лобби",
                    PlayerId = playerId
                });
                return msg;
            }
            //TODO: isLobbyactive
            LobbyService.AddUser(playerId);
            if (LobbyService.EmptyPlaceCount(playerId) != 0)
            {
                msg.Add(new MessageConfig
                {
                    Answer = $"Вы добавлены в лобби, осталось игроков для начала игры{LobbyService.EmptyPlaceCount(playerId)} " +
                             $"\n /stop - для остановки поиска лобби" +
                             $"\n /help - попросить совет",
                    PlayerId = playerId
                });
                var character = characterRepository.Read(playerId);
                character.State = CharacterState.FindGame;
                CharacterRepository.Update(character);
                return msg;
            }
            LobbyService.StartNewLobby(playerId);
            var memberlist = members.ReadMemberList(
            members.ReadLobbyId(playerId));
            string s = "";
            List<string> Names = new List<string>();
            var characters = CharacterRepository.ReadAll();
            foreach (var member in memberlist)
            {
               s += "*" + (characters.Find(e => e.TelegramUserId == member.UserId).CharacterName) + "*" + ", ";
            }

            s = s.Substring(0, s.Length - 2);
            s += ".";
;            foreach (var item in memberlist)
            {
                msg.Add(new MessageConfig
                {
                    Answer = "Игра начата. Игроки: " + s + "" +
                             "\n/leave - испортить другим игру" +
                             "\n/afk - если кто-то долго не ходит \n",
                    PlayerId = item.UserId,
                });
                item.IsLobbyActive = true;
                members.Update(item);
            }
            foreach (var item in memberlist.Select(e => e.UserId))
            {
                var character = characterRepository.Read(item);
                character.State = CharacterState.InGame;
                characterRepository.Update(character);
            }

            msg.Find(e => e.PlayerId == members.ReadMemberList(members.ReadLobbyId(playerId)).First().UserId).Answer += " Ваш ход";
            msg.Find(e => e.PlayerId == members.ReadMemberList(members.ReadLobbyId(playerId)).First().UserId).KeyBoardId
                = KeyboardType.Move;

            return msg;
        }

        public static KeyboardType GetKeyboardType(Player player)
        {
            if (player.Bombs != 0 && player.Guns != 0)
                return KeyboardType.ShootwithBomb;
            if (player.Bombs == 0 && player.Guns == 0)
                return KeyboardType.Move;
            if (player.Bombs != 0 && player.Guns == 0)
                return KeyboardType.Bomb;
            if (player.Bombs == 0 && player.Guns != 0)
                return KeyboardType.Shoot;
            return KeyboardType.Move;
        }
    }
}