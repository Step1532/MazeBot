using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace MazeGenerator.TeleBot
{
    public class MazeBot
    {
        private readonly LobbyControl _lobbyControl = new LobbyControl();
        public readonly TelegramBotClient BotClient;

        public int stroke = 1;

        public MazeBot(string _tMaze)
        {
            //object sender =
            //    JsonConvert.DeserializeObject(
            //        "{\"Timeout\":\"00:01:40\",\"IsReceiving\":true,\"MessageOffset\":0}"); // {"Message":{"message_id":603,"from":{"id":310811454,"is_bot":false,"first_name":"Step1","language_code":"ru"},"date":1532032618,"chat":{"id":-1001317216792,"type":"supergroup","title":"Если что, переименую"},"text":"/up","entities":[{"type":"bot_command","offset":0,"length":3}]}}

            //MessageEventArgs e =
            //    JsonConvert.DeserializeObject<MessageEventArgs>(
            //        "  {\"Message\":{\"message_id\":603,\"from\":{\"id\":310811454,\"is_bot\":false,\"first_name\":\"Step1\",\"language_code\":\"ru\"},\"date\":1532032618,\"chat\":{\"id\":-1001317216792,\"type\":\"supergroup\",\"title\":\"Если что, переименую\"},\"text\":\"/up\",\"entities\":[{\"type\":\"bot_command\",\"offset\":0,\"length\":3}]}}"); // {"Message":{"message_id":603,"from":{"id":310811454,"is_bot":false,"first_name":"Step1","language_code":"ru"},"date":1532032618,"chat":{"id":-1001317216792,"type":"supergroup","title":"Если что, переименую"},"text":"/up","entities":[{"type":"bot_command","offset":0,"length":3}]}}
            //BotClient = new TelegramBotClient(_tMaze); //{"Timeout":"00:01:40","IsReceiving":true,"MessageOffset":0}
            //BotClient.OnMessage += OnNewMessage;
            //BotClient.StartReceiving();
            //OnNewMessage(sender, e);
        }

        //TODO: return result info after action
        public void ExecuteCommand(long chatId, string command, int fromId)
        {
            if (command == "/start")
                if (chatId == fromId)
                    Console.WriteLine(_lobbyControl.GenerateLink(fromId));

            if (command == "/add")
            {
                var lobbyId = _lobbyControl.GetLobbyId(chatId);
                //TODO: check if already in lobby
                _lobbyControl.AddPlayer(fromId, lobbyId);

                //TODO: Вынести логику, добавть статические всему
                //TODO переделать, ограничение, разбан чата
                //BotClient.SendTextMessageAsync(e.Message.Chat.Id, game.CheckStartGame(lobbyList[lobbydId - 1], lobbydId),
                //                        ParseMode.Markdown);
                //TODO: mb reply message?
                //else { BotClient.DeleteMessageAsync(e.Message.Chat.Id, e.Message.MessageId); }
            }

            //TODO: написать команды + направление движения
            if (command == "/up" && chatId != fromId)
            {
                //string s = FormatAnswers.AnswerUp(_mazeLogic.TryMove(e.Message.From.Id, a.GetLobbyId(e.Message.From.Id)),
                //e.Message.From.Username);
                //BotClient.SendTextMessageAsync(e.Message.Chat.Id, s, ParseMode.Markdown);

                stroke++;
                //TODO:
            }

            if (command == "/getinfo")
            {
            }
        }


        public void OnNewMessage(object sender, MessageEventArgs e)
        {
            //Console.WriteLine(JsonConvert.SerializeObject(e));
            //Console.WriteLine(JsonConvert.SerializeObject(sender));

            if (e.Message.Type != MessageType.Text)
                return;
            if (e.Message.Text == "/start")
                if (e.Message.Chat.Id == e.Message.From.Id)
                {
                    BotClient.SendTextMessageAsync(e.Message.Chat.Id, _lobbyControl.GenerateLink(e.Message.From.Id),
                        ParseMode.Markdown);
                    Console.WriteLine("good");
                }

            if (e.Message.Text == "/add") ExecuteCommand(e.Message.Chat.Id, e.Message.Text, e.Message.From.Id);

            //TODO: написать команды + направление движения
            if (e.Message.Text == "/up" && e.Message.Chat.Id != e.Message.From.Id)
                ExecuteCommand(e.Message.Chat.Id, e.Message.Text, e.Message.From.Id);

            if (e.Message.Text == "/getinfo") ExecuteCommand(e.Message.Chat.Id, e.Message.Text, e.Message.From.Id);
        }
    }
}