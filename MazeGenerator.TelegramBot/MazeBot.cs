﻿using System;
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
            Console.ReadLine();
        }

        public void OnNewMessage(object sender, MessageEventArgs e)
        {
            int playerId = e.Message.From.Id;
            var character = _characterRepository.Read(playerId);


            if (e.Message.Type != MessageType.Text)
                return;

            //=====

            MessageConfig msg;
            if (character == null)
            {
                 msg = StateMachine(CharacterState.NewCharacter, e.Message.Text, playerId);
            }
            else
            {
                 msg = StateMachine(_characterRepository.Read(playerId).State, e.Message.Text, playerId);
            }

            if (msg != null)
            {
                var keyboard = GetKeyboardMarkup(KeyboardType.Move);

                if (msg.OtherPlayersId != null)
                {
                    foreach (var playerid in msg.OtherPlayersId)
                    {
                        BotClient.SendTextMessageAsync(playerid, msg.AnswerForOther, ParseMode.Markdown, false, false,
                            0, keyboard);
                    }

                    BotClient.SendTextMessageAsync(msg.NextPlayerId, "Ваш Ход");
                }

                BotClient.SendTextMessageAsync(msg.CurrentPlayerId, msg.Answer, ParseMode.Markdown);
                return;
            }


            BotClient.SendChatActionAsync(playerId, ChatAction.Typing);
        }

        //TODO: проверить кто есть sender
        //Если бот клиент, то можно упростить
        private void BotClient_OnCallbackQueryShoot(object sender, CallbackQueryEventArgs e)
        {
            var bot = (TelegramBotClient) sender;
            bot.OnCallbackQuery -= BotClient_OnCallbackQueryShoot;
            switch (e.CallbackQuery.Data)
            {
                case "1":
                    SComm(e, Direction.North);
                    break;
                case "2":
                    SComm(e, Direction.West);
                    break;
                case "3":
                    SComm(e, Direction.East);
                    break;
                case "4":
                    SComm(e, Direction.South);
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
                    BComm(e, Direction.North);
                    break;
                case "2":
                    BComm(e, Direction.West);
                    break;
                case "3":
                    BComm(e, Direction.East);
                    break;
                case "4":
                    BComm(e, Direction.South);
                    break;
                default:
                    bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id,
                        "Выбирайте направление а не пустые кнопки");
                    break;
            }
        }
        //TODO: переписать название и арнументы методов
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
                    throw new ArgumentException(keyBoardId.ToString());
            }
        }

        public void SComm(CallbackQueryEventArgs e, Direction direction)
        {
            var s = BotService.ShootCommand(e.CallbackQuery.From.Id, direction);
            BotClient.SendTextMessageAsync(e.CallbackQuery.From.Id, s.Answer, ParseMode.Default, false, false, 0,
            KeybordConfiguration.NewKeyBoard());
        }
        public void BComm(CallbackQueryEventArgs e, Direction direction)
        {
            var s = BotService.BombCommand(e.CallbackQuery.From.Id, direction);
            BotClient.SendTextMessageAsync(e.CallbackQuery.From.Id, s.Answer, ParseMode.Default, false, false, 0,
                KeybordConfiguration.NewKeyBoard());
           
        }

        public MessageConfig StateMachine(CharacterState state, string command, int playerId)
        {
            switch (state)
            {
                case CharacterState.ChangeName:
                    return BotService.TryChangeName(command, playerId);

                case CharacterState.ChangeGameMode:
                    if (command == "/game")
                    {
                        return StateMachineService.FindGameCommand(playerId);
                    }
                    else if (command == "/tutorial")
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        return new MessageConfig()
                        {
                            Answer = "неверная команда",
                            CurrentPlayerId = playerId
                        };
                    }

                case CharacterState.Tutorial:
                    throw new NotImplementedException();
                    if (command == "Вверх")
                    {
                        throw new NotImplementedException();
                        //TODO: те же команды что и в Ingame, только по другому обработать
                    }
                    else if (command == "/skiptutorial")
                    {
                        throw new NotImplementedException();
                        //TODO: выйти из туториала
                    }
                    else
                    {
                        throw new NotImplementedException();
                        //TODO: сообщение, что неверная комманда
                    }

                case CharacterState.FindGame:
                    if (command == "/help")
                    {
                        throw new NotImplementedException();
                    }
                    else if (command == "/stop")
                    {
                        _characterRepository.Read(playerId);
                        MemberRepository repo =new MemberRepository();
                        repo.DeleteOne(playerId);
                        return new MessageConfig()
                        {
                            Answer = "Вы удалены из очереди",
                            CurrentPlayerId = playerId
                        };
                    }
                    else
                    {
                        throw new NotImplementedException();
                        //TODO: сообщение, что неверная комманда
                    }

                case CharacterState.InGame:
                    if (command == "Вверх")
                    {
                        return BotService.MoveCommand(playerId, Direction.North);
                    }
                    if (command == "Вниз")
                    {
                        return BotService.MoveCommand(playerId, Direction.South);
                    }
                    if (command == "Вправо")
                    {
                        return BotService.MoveCommand(playerId, Direction.East);
                    }
                    if (command == "Влево")
                    {
                        return BotService.MoveCommand(playerId, Direction.West);
                    }
                    if (command == "Удар кинжалом")
                    {
                        return BotService.StabCommand(playerId);
                    }
                    if (command == "Пропуск хода")
                    {
                        return BotService.SkipTurn(playerId);
                    }

                    if (command == "Выстрел")
                    {
                        var inlineKeyboard = KeybordConfiguration.ChooseDirectionKeyboard();
                        BotClient.SendTextMessageAsync(playerId, "Выбирай направление", replyMarkup: inlineKeyboard);
                        BotClient.OnCallbackQuery += BotClient_OnCallbackQueryShoot;
                        return null;
                    }
                    if (command == "Взрыв стены")
                    {
                        var inlineKeyboard = KeybordConfiguration.ChooseDirectionKeyboard();
                        BotClient.SendTextMessageAsync(playerId, "Выбирай направление", replyMarkup: inlineKeyboard);
                        BotClient.OnCallbackQuery += BotClient_OnCallbackQueryBomb;
                        return null;
                    }
                    if (command == "/afk")
                    {
                        return BotService.AfkCommand(playerId);
                    }
                    return new MessageConfig()
                    {
                        Answer = Answers.UndefinedCommand.RandomAnswer()
                    };

                case CharacterState.NewCharacter:
                    if (command == "/start")
                    {
                        if (_characterRepository.Read(playerId) == null)
                        {
                            _characterRepository.Create(playerId);
                            return new MessageConfig()
                            {
                                Answer = "Напишите имя персонажа",
                                CurrentPlayerId = playerId
                            };
                        }

                        return new MessageConfig()
                        {
                            Answer = "Вы хотите удалить персонажа? Для удаления напишите *Удаляю* и нажмите /start",
                            CurrentPlayerId = playerId
                        };
                    }
                    return new MessageConfig()
                    {
                        Answer = "неверная команда",
                        CurrentPlayerId = playerId
                    };

                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
               
            }
            throw new Exception();
        }
    }
}