using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MazeGenerator.Core;
using MazeGenerator.Core.Services;
using MazeGenerator.Core.Tools;
using MazeGenerator.Database;
using MazeGenerator.Models;
using MazeGenerator.Models.Enums;
using MazeGenerator.TelegramBot.Models;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Requests;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MazeGenerator.TelegramBot
{
    public class MazeBot
    {
        public readonly TelegramBotClient BotClient;
        private readonly CharacterRepository _characterRepository = new CharacterRepository();


        public MazeBot(string token)
        {
            BotClient = new TelegramBotClient(token); //{"Timeout":"00:01:40","IsReceiving":true,"MessageOffset":0}
            BotClient.OnMessage += OnNewMessage;
            BotClient.StartReceiving();
        }

        public void OnNewMessage(object sender, MessageEventArgs e)
        {
            
            int playerId = e.Message.From.Id;
            var character = _characterRepository.Read(playerId);


            if (e.Message.Type != MessageType.Text)
                return;
            BotClient.SendChatActionAsync(playerId, ChatAction.Typing);

            
            List<MessageConfig> msg;
            //try
            //{

            //    if (character == null)
            //    {
            //        msg = StateMachine(CharacterState.NewCharacter, e.Message.Text, playerId);
            //    }
            //    else
            //    {
            //        msg = StateMachine(_characterRepository.Read(playerId).State, e.Message.Text, playerId);
            //    }
            //}
            //catch (Exception exception)
            //{
            //    Console.WriteLine(exception);
            //    return;
            //}

            if (character == null)
            {
                msg = StateMachine(CharacterState.NewCharacter, e.Message.Text, playerId);
            }
            else
            {
                msg = StateMachine(_characterRepository.Read(playerId).State, e.Message.Text, playerId);
            }

            if (msg == null) return;
            foreach (var item in msg)
            {
                if (item.KeyBoardId != null)
                {
                    BotClient.SendTextMessageAsync(item.PlayerId, item.Answer, ParseMode.Markdown, false,false,0, GetKeyboardMarkup(item.KeyBoardId));
                }
                else
                {
                    BotClient.SendTextMessageAsync(item.PlayerId, item.Answer, ParseMode.Markdown);
                }

            }
        }

        private void BotClient_OnCallbackQueryShoot(object sender, CallbackQueryEventArgs e)
        {
            var bot = (TelegramBotClient) sender;
            bot.OnCallbackQuery -= BotClient_OnCallbackQueryShoot;
            switch (e.CallbackQuery.Data)
            {
                case "1":
                    SComm(e.CallbackQuery.From.Id, Direction.North);
                    break;
                case "2":
                    SComm(e.CallbackQuery.From.Id, Direction.West);
                    break;
                case "3":
                    SComm(e.CallbackQuery.From.Id, Direction.East);
                    break;
                case "4":
                    SComm(e.CallbackQuery.From.Id, Direction.South);
                    break;
                default:
                    bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id,
                        "Выбирайте направление а не пустые кнопки");
                    break;
            }
        }

        private void BotClient_OnCallbackQueryBomb(object sender, CallbackQueryEventArgs e)
        {
            var bot = (TelegramBotClient) sender;
            bot.OnCallbackQuery -= BotClient_OnCallbackQueryBomb;
            switch (e.CallbackQuery.Data)
            {
                case "1":
                    BComm(e.CallbackQuery.From.Id, Direction.North);
                    break;
                case "2":
                    BComm(e.CallbackQuery.From.Id, Direction.West);
                    break;
                case "3":
                    BComm(e.CallbackQuery.From.Id, Direction.East);
                    break;
                case "4":
                    BComm(e.CallbackQuery.From.Id, Direction.South);
                    break;
                default:
                    bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id,
                        "Выбирайте направление а не пустые кнопки");
                    break;
            }
        }
        public ReplyKeyboardMarkup GetKeyboardMarkup(KeyboardType keyBoardId)
        {
            switch (keyBoardId)
            {
                case KeyboardType.Bomb:
                    return KeybordConfiguration.WithoutShootKeyBoard();
                case KeyboardType.Move:
                    return KeybordConfiguration.WithoutBombAndShootKeyboard();
                case KeyboardType.Shoot:
                    return KeybordConfiguration.WithoutBombKeyBoard();
                case KeyboardType.ShootwithBomb:
                    return KeybordConfiguration.NewKeyBoard();
                default:
                    return null;
            }
        }

        public void SComm(int playerid, Direction direction)
        {
            var s = BotService.ShootCommand(playerid, direction);
            //TODO: Вынести в метод SendResponse
            foreach (var item in s)
            {
                if (item.KeyBoardId != null)
                {
                    BotClient.SendTextMessageAsync(item.PlayerId, item.Answer, ParseMode.Markdown, false, false, 0, GetKeyboardMarkup(item.KeyBoardId));
                }
                else
                {
                    BotClient.SendTextMessageAsync(item.PlayerId, item.Answer, ParseMode.Markdown);
                }

            }
        }
        public void BComm(int playerId, Direction direction)
        {
            var s = BotService.BombCommand(playerId, direction);
            foreach (var item in s)
            {
                if (item.KeyBoardId != null)
                {
                    BotClient.SendTextMessageAsync(item.PlayerId, item.Answer, ParseMode.Markdown, false, false, 0, GetKeyboardMarkup(item.KeyBoardId));
                }
                else
                {
                    BotClient.SendTextMessageAsync(item.PlayerId, item.Answer, ParseMode.Markdown);
                }

            }
        }

        public List<MessageConfig> StateMachine(CharacterState state, string command, int playerId)
        {
            //TODO: Вынести сюда создание репозиториев
            switch (state)
            {
                case CharacterState.ChangeName:
                    return BotService.TryChangeName(command, playerId);

                case CharacterState.ChangeGameMode:
                    switch (command)
                    {
                        case "/game":
                            return BotService.FindGameCommand(playerId);
                        case "/tutorial":
                            var character = _characterRepository.Read(playerId);
                            LobbyRepository lobbyRepository = new LobbyRepository();
                            MemberRepository memberRepository = new MemberRepository();
                            memberRepository.Create(0, playerId);
                            var lobby = lobbyRepository.Read(0);
                            lobby.Players.Add(new Player
                            {
                                Health = 3,
                                HeroName = character.CharacterName,
                                Rotate = Direction.North,
                                TelegramUserId = playerId,
                                UserCoordinate = new Coordinate(3, 3)
                            });
                            lobbyRepository.Update(lobby);
                            character.State = CharacterState.Tutorial;
                            _characterRepository.Update(character);
                            return new List<MessageConfig>
                            {
                                new MessageConfig()
                                {
                                    Answer = "Обучение",
                                    PlayerId = playerId
                                }
                            };
                        default:
                            return new List<MessageConfig>
                            {
                                new MessageConfig()
                                {
                                    Answer = "неверная команда",
                                    PlayerId = playerId
                                }
                            };
                    }

                case CharacterState.Tutorial:
                    switch (command)
                    {
                        case "Вверх":
                            return TutorialService.MoveCommand(playerId, Direction.North);
                        case "Вниз":
                            return TutorialService.MoveCommand(playerId, Direction.South);
                        case "Вправо":
                            return TutorialService.MoveCommand(playerId, Direction.East);
                        case "Влево":
                            return TutorialService.MoveCommand(playerId, Direction.West);
                        case "Взрыв стены":
                            var inlineKeyboard = KeybordConfiguration.ChooseDirectionKeyboard();
                            BotClient.SendTextMessageAsync(playerId, "Выбирай направление",
                                replyMarkup: inlineKeyboard);
                            BotClient.OnCallbackQuery += BotClient_OnCallbackQueryBomb;
                            return null;
                        case "/skip":
                            var character = _characterRepository.Read(playerId);
                            LobbyRepository lobbyRepository = new LobbyRepository();
                            MemberRepository memberRepository = new MemberRepository();
                            memberRepository.DeleteOne(playerId);
                            var lobby = lobbyRepository.Read(0);
                            lobby.Players.Remove(lobby.Players.Find(e => e.TelegramUserId == playerId));
                            lobbyRepository.Update(lobby);
                            character.State = CharacterState.ChangeGameMode;
                            _characterRepository.Update(character);
                            return new List<MessageConfig>
                            {
                                new MessageConfig()
                                {
                                    Answer = "Обучение пропущено",
                                    PlayerId = playerId
                                }
                            };
                        default:
                            return new List<MessageConfig>
                            {
                                new MessageConfig()
                                {
                                    Answer = "Неверная команда",
                                    PlayerId = playerId
                                }
                            };
                    }

                case CharacterState.FindGame:
                    switch (command)
                    {
                        case "/help":
                            throw new NotImplementedException();
                        case "/stop":
                            _characterRepository.Read(playerId);
                            MemberRepository repo = new MemberRepository();
                            var character = _characterRepository.Read(playerId);
                            character.State = CharacterState.ChangeGameMode;
                            _characterRepository.Update(character);
                            repo.DeleteOne(playerId);
                            return new List<MessageConfig>
                            {
                                new MessageConfig()
                                {
                                    Answer = "Вы удалены из очереди",
                                    PlayerId = playerId
                                }
                            };
                        default:
                            return new List<MessageConfig>
                            {
                                new MessageConfig()
                                {
                                    Answer = "Неверная команда",
                                    PlayerId = playerId
                                }
                            };
                    }

                case CharacterState.InGame:
                    switch (command)
                    {
                        case "Вверх":
                            return BotService.MoveCommand(playerId, Direction.North);
                        case "Вниз":
                            return BotService.MoveCommand(playerId, Direction.South);
                        case "Вправо":
                            return BotService.MoveCommand(playerId, Direction.East);
                        case "Влево":
                            return BotService.MoveCommand(playerId, Direction.West);
                        case "Удар кинжалом":
                            return BotService.StabCommand(playerId);
                        case "Пропуск хода":
                            return BotService.SkipTurn(playerId);
                        case "Выстрел":
                        {
                            var inlineKeyboard = KeybordConfiguration.ChooseDirectionKeyboard();
                            BotClient.SendTextMessageAsync(playerId, "Выбирай направление",
                                replyMarkup: inlineKeyboard);
                            BotClient.OnCallbackQuery += BotClient_OnCallbackQueryShoot;
                            return null;
                        }
                        case "Взрыв стены":
                        {
                            var inlineKeyboard = KeybordConfiguration.ChooseDirectionKeyboard();
                            BotClient.SendTextMessageAsync(playerId, "Выбирай направление",
                                replyMarkup: inlineKeyboard);
                            BotClient.OnCallbackQuery += BotClient_OnCallbackQueryBomb;
                            return null;
                        }
                        case "/afk":
                            return BotService.AfkCommand(playerId);
                        case "/leave":
                            return BotService.TryLeaveCommand(playerId);
                    }

                    return new List<MessageConfig>
                    {
                        new MessageConfig()
                        {
                            Answer = Answers.UndefinedCommand.RandomAnswer(),
                            PlayerId = playerId
                        }
                    };

                case CharacterState.NewCharacter:
                    if (command == "/start")
                    {
                        if (_characterRepository.Read(playerId) == null)
                        {
                            _characterRepository.Create(playerId);
                            return new List<MessageConfig>
                            {
                                new MessageConfig()
                                {
                                    Answer = "Напишите имя персонажа",
                                    PlayerId = playerId
                                }
                            };
                        }
                    }

                    return new List<MessageConfig>
                    {
                        new MessageConfig()
                        {
                            Answer = "Неверная команда",
                            PlayerId = playerId
                        }
                    };
                case CharacterState.AcceptLeave:
                    if (command == "Подтверждаю")
                    {
                        return BotService.LeaveCommand(playerId);
                    }
                    else
                    {
                        return new List<MessageConfig>
                        {
                            new MessageConfig
                            {
                                Answer = "Не подтверждено",
                                PlayerId = playerId
                            }
                        };
                    }
                case CharacterState.Ban:
                    return new List<MessageConfig>
                    {
                        new MessageConfig()
                        {
                            Answer = "В бане",
                            PlayerId = playerId
                        }
                    };

                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);

            }
            throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}