using System;
using MazeGenerator.Models.Enums;
using MazeGenerator.TelegramBot.Models;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MazeGenerator.TelegramBot
{
    public class MazeBot
    {
        public readonly TelegramBotClient BotClient;


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

            //TODO: при начале игры говорить всем куда игрок попал
            if (e.Message.Type != MessageType.Text)
                return;

            if (e.Message.Text == "/start")
            {
                BotClient.SendChatActionAsync(playerId, ChatAction.Typing);
                //TODO: тут будет турториал
//                var inlineKeyboard = BotTools.NewInlineKeyBoardForChooseDirection();
//                BotClient.SendTextMessageAsync(playerId, "Выбирай направление", replyMarkup: inlineKeyboard);
//                BotClient.OnCallbackQuery += BotClient_OnCallbackQuery;
                BotClient.SendTextMessageAsync(playerId, "проверка связи");
                return;
            }
            if (e.Message.Text == "/game")
            {
                if(LobbyControl.CheckLobby(playerId))
                {
                    BotClient.SendTextMessageAsync(playerId, "Вы уже находитесь в лобби");
                }
                else
                {
                    LobbyControl.AddUser(playerId);
                    if (LobbyControl.EmptyPlaceCount(playerId) == 0)
                    {
                        BotService.StartGame(playerId);
                        BotClient.SendTextMessageAsync(playerId, "Игра начата");
                    }
                    else
                    {
                        string m =
                            $"Вы добавлены в лобби, осталось игроков для начала игры{LobbyControl.EmptyPlaceCount(playerId)}";
                        BotClient.SendTextMessageAsync(playerId, m);
                    }
                }
            }
            if (!LobbyControl.CheckLobby(playerId))
                return;
            if (e.Message.Text == "Вверх")
            {
                BotClient.SendChatActionAsync(playerId, ChatAction.Typing);
                MComm(e, Direction.North);
            }
            if (e.Message.Text == "Вниз")
            {
                BotClient.SendChatActionAsync(playerId, ChatAction.Typing);
                MComm(e, Direction.South);
            }
            if (e.Message.Text == "Влево")
            {
                BotClient.SendChatActionAsync(playerId, ChatAction.Typing);
                MComm(e, Direction.West);
            }
            if (e.Message.Text == "Вправо")
            {
                BotClient.SendChatActionAsync(playerId, ChatAction.Typing);
                MComm(e, Direction.East);
            }
            if (e.Message.Text == "Удар кинжалом")
            {
                BotClient.SendChatActionAsync(playerId, ChatAction.Typing);
                //Todo: нет метода кинжала
            }
            if (e.Message.Text == "Пропуск хода")
            {
                BotClient.SendChatActionAsync(playerId, ChatAction.Typing);

            }
            if (e.Message.Text == "Выстрел")
            {
                BotClient.SendChatActionAsync(playerId, ChatAction.Typing);
                var inlineKeyboard = KeybordConfiguration.ChooseDirectionKeyboard();
                BotClient.SendTextMessageAsync(playerId, "Выбирай направление", replyMarkup: inlineKeyboard);
                BotClient.OnCallbackQuery += BotClient_OnCallbackQueryShoot;


            }
            if (e.Message.Text == "Взрыв стены")
            {
                BotClient.SendChatActionAsync(playerId, ChatAction.Typing);
                var inlineKeyboard = KeybordConfiguration.ChooseDirectionKeyboard();
                BotClient.SendTextMessageAsync(playerId, "Выбирай направление", replyMarkup: inlineKeyboard);
                BotClient.OnCallbackQuery += BotClient_OnCallbackQueryBomb;
            }

        }

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
        public void MComm(MessageEventArgs e, Direction direction)
        {
            var s = BotService.MoveCommand(e.Message.From.Id, direction, e.Message.From.Username);
            if (s.KeyBoardId != KeyBoardEnum.Move)
            {
                BotClient.SendTextMessageAsync(e.Message.From.Id, s.Answer, ParseMode.Default, false, false, 0,
                    KeybordConfiguration.NewKeyBoard());
            }
            else
            {
                BotClient.SendTextMessageAsync(e.Message.From.Id, s.Answer, ParseMode.Markdown);
            }
        }
        public void SComm(CallbackQueryEventArgs e, Direction direction)
        {
            var s = BotService.ShootCommand(e.CallbackQuery.From.Id, direction, e.CallbackQuery.From.Username);
            BotClient.SendTextMessageAsync(e.CallbackQuery.From.Id, s.Answer, ParseMode.Default, false, false, 0,
            KeybordConfiguration.NewKeyBoard());
        }
        public void BComm(CallbackQueryEventArgs e, Direction direction)
        {
            var s = BotService.BombCommand(e.CallbackQuery.From.Id, direction, e.CallbackQuery.From.Username);
            BotClient.SendTextMessageAsync(e.CallbackQuery.From.Id, s.Answer, ParseMode.Default, false, false, 0,
                KeybordConfiguration.NewKeyBoard());
        }
    }
}