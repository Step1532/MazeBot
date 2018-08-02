using System;
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
        CharacterRepository _characterRepository = new CharacterRepository();


        public MazeBot(string _tMaze)
        {
            BotClient = new TelegramBotClient(_tMaze); //{"Timeout":"00:01:40","IsReceiving":true,"MessageOffset":0}
            BotClient.OnMessage += OnNewMessage;
            BotClient.StartReceiving();
            Console.ReadLine();
        }

        public void OnNewMessage(object sender, MessageEventArgs e)
        {
            int playerId = e.Message.From.Id;
            var character = _characterRepository.Read(playerId);


			//TODO: вынести метод валидации
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
            //TODO: move to new method 
            ReplyKeyboardMarkup keyboard = new ReplyKeyboardMarkup();
            if (msg.KeyBoardId == KeyBoardEnum.Bomb)
            {
                keyboard = KeybordConfiguration.WithoutShootKeyBoard();
            }
            else if (msg.KeyBoardId == KeyBoardEnum.Move)
            {
                keyboard = KeybordConfiguration.WithoutBombAndShootKeyboard();
            }
            else if (msg.KeyBoardId == KeyBoardEnum.Shoot)
            {
                keyboard = KeybordConfiguration.WithoutBombKeyBoard();
            }
            else if (msg.KeyBoardId == KeyBoardEnum.ShootwithBomb)
            {
                keyboard = KeybordConfiguration.NewKeyBoard();
            }
            if (msg.OtherPlayersId != null)
            {
                foreach (var playerid in msg.OtherPlayersId)
                {
                    BotClient.SendTextMessageAsync(playerid, msg.AnswerForOther, ParseMode.Markdown, false, false, 0, keyboard);
                }
                BotClient.SendTextMessageAsync(msg.NextPlayerId, "Ваш Ход");
            }
            BotClient.SendTextMessageAsync(msg.CurrentPlayerId, msg.Answer, ParseMode.Markdown);
            return;
            


            BotClient.SendChatActionAsync(playerId, ChatAction.Typing);

			//TODO: вызов стейт машин



        }

        //TODO: проверить кто есть sender
        //Если бот клиент, то можно упростить
        private void BotClient_OnCallbackQueryShoot(object sender, CallbackQueryEventArgs e)
        {
            if (e.CallbackQuery.Data != "0")
            {
                BotClient.OnCallbackQuery -= BotClient_OnCallbackQueryShoot;
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
                }
            }
            else
            {
                BotClient.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id,
                    "Выбирайте направление а не пустые кнопки");
            }
        }
        private void BotClient_OnCallbackQueryBomb(object sender, CallbackQueryEventArgs e)
        {
            if (e.CallbackQuery.Data != "0")
            {
                BotClient.OnCallbackQuery -= BotClient_OnCallbackQueryBomb;
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
                }
            }
            else
            {
                BotClient.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id,
                    "Выбирайте направление а не пустые кнопки");
            }
        }
        //TODO: следующим методам добавить, что б отправляли всем игрокам
		//TODO: переписать название и арнументы методов
        public void SComm(CallbackQueryEventArgs e, Direction direction)
        {
            var s = BotService.ShootCommand(e.CallbackQuery.From.Id, direction, e.CallbackQuery.From.Username);
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
                    return TryChangeName(command, playerId);

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
                        throw new NotImplementedException();
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
                        throw new NotImplementedException();
                        //TODO: прекратить поиск
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

        //TODO: move to service
        private MessageConfig TryChangeName(string username, int playerId)
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

            if (_characterRepository.ReadAll().Any(e => e.CharacterName == username))
            {
                return new MessageConfig()
                {
                    CurrentPlayerId = playerId,
                    Answer = $"Имя существует"
                };
            }

            var r = _characterRepository.Read(playerId);
            r.CharacterName = username;
            r.State = CharacterState.ChangeGameMode;
            _characterRepository.Update(r);

            return new MessageConfig()
            {
                CurrentPlayerId = playerId,
                Answer = $"Имя _{username}_задано"
            };
        }
    }
}