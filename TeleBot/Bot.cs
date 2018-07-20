using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Telegram.Bot;
using System.Threading.Tasks;
using MazeGenerator.NewGame;
using MazeGenerator.TeleBot;
using MazeGenerator.Tools;
using MazeGenerator.MazeLogic;
using Newtonsoft.Json;
using Telegram.Bot.Args;
using Telegram.Bot.Requests;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
namespace MazeGenerator.TeleBot
{
    public class MazeBot
    {
        public readonly TelegramBotClient Bot;
        public LobbyControl a = new LobbyControl();
        public Player player = new Player();
        public Rules ruls = new Rules();
        public MazeLogic.MazeLogic Act = new MazeLogic.MazeLogic();
        public int stroke = 1;

        public MazeBot(string _tMaze)
        {
            //object sender =
            //    JsonConvert.DeserializeObject(
            //        "{\"Timeout\":\"00:01:40\",\"IsReceiving\":true,\"MessageOffset\":0}"); // {"Message":{"message_id":603,"from":{"id":310811454,"is_bot":false,"first_name":"Step1","language_code":"ru"},"date":1532032618,"chat":{"id":-1001317216792,"type":"supergroup","title":"Если что, переименую"},"text":"/up","entities":[{"type":"bot_command","offset":0,"length":3}]}}

            //MessageEventArgs e =
            //    JsonConvert.DeserializeObject<MessageEventArgs>(
            //        "  {\"Message\":{\"message_id\":603,\"from\":{\"id\":310811454,\"is_bot\":false,\"first_name\":\"Step1\",\"language_code\":\"ru\"},\"date\":1532032618,\"chat\":{\"id\":-1001317216792,\"type\":\"supergroup\",\"title\":\"Если что, переименую\"},\"text\":\"/up\",\"entities\":[{\"type\":\"bot_command\",\"offset\":0,\"length\":3}]}}"); // {"Message":{"message_id":603,"from":{"id":310811454,"is_bot":false,"first_name":"Step1","language_code":"ru"},"date":1532032618,"chat":{"id":-1001317216792,"type":"supergroup","title":"Если что, переименую"},"text":"/up","entities":[{"type":"bot_command","offset":0,"length":3}]}}
            //Bot = new TelegramBotClient(_tMaze); //{"Timeout":"00:01:40","IsReceiving":true,"MessageOffset":0}
            //Bot.OnMessage += OnNewMessage;
            //Bot.StartReceiving();
            //OnNewMessage(sender, e);
        }

        public void f(int chatId, string text, int fromId)
        {
            if (text == "/start")
            {
                if (chatId == fromId)
                {
                    Console.WriteLine(a.GenerateLink(fromId));
                }
            }

            if (text == "/add")
            {

                int lobbydId = a.GetLobbyId(chatId);
                //                    var lobbyList = PJson.();
                //TODO: Вынести логику, добавть статические всему
                //TODO переделать, ограничение, разбан чата
                //                if (lobbyList[lobbydId - 1] != ruls.RulesList[0]) 
                {
                    //                    lobbyList[lobbydId - 1]++;
                    //                    player.AddNewPlayer(lobbyList[lobbydId - 1], e.Message.From.Id, lobbydId);
                    NewGames game = new NewGames();
                    //                    Bot.SendTextMessageAsync(e.Message.Chat.Id, game.CheckStartGame(lobbyList[lobbydId - 1], lobbydId),
                    //                        ParseMode.Markdown);

                    Console.WriteLine("good");
                }
                //                else { Bot.DeleteMessageAsync(e.Message.Chat.Id, e.Message.MessageId); }

            }

            //TODO: написать команды + направление движения
            if (text == "/up" && chatId != fromId)
            {
                //                    string s = FormatAnswers.AnswerUp(Act.TryMove(e.Message.From.Id, a.GetLobbyId(e.Message.From.Id)),
                //                    e.Message.From.Username);
                //                    Bot.SendTextMessageAsync(e.Message.Chat.Id, s, ParseMode.Markdown);
                Console.WriteLine("good");
                stroke++;
                if (stroke > ruls.RulesList[0]) stroke = 1;
            }

            if (text == "/getinfo")
            {
                //                NewGames.StartGame();
                Console.WriteLine("good");
            }


        }
    

         public void OnNewMessage(object sender, MessageEventArgs e)
        {
//            Console.WriteLine(JsonConvert.SerializeObject(e));
//            Console.WriteLine(JsonConvert.SerializeObject(sender));

            if (e.Message.Type != MessageType.Text) return;
            if (e.Message.Text == "/start")
            {
                if (e.Message.Chat.Id == e.Message.From.Id)
                {
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, a.GenerateLink(e.Message.From.Id), ParseMode.Markdown);
                    Console.WriteLine("good");

                }
            }
            if (e.Message.Text == "/add")
            {

                    int lobbydId = a.GetLobbyId(e.Message.Chat.Id);
//                    var lobbyList = PJson.();
                //TODO: Вынести логику, добавть статические всему
                //TODO переделать, ограничение, разбан чата
//                if (lobbyList[lobbydId - 1] != ruls.RulesList[0]) 
                {
//                    lobbyList[lobbydId - 1]++;
//                    player.AddNewPlayer(lobbyList[lobbydId - 1], e.Message.From.Id, lobbydId);
                    NewGames game = new NewGames();
//                    Bot.SendTextMessageAsync(e.Message.Chat.Id, game.CheckStartGame(lobbyList[lobbydId - 1], lobbydId),
//                        ParseMode.Markdown);
                   
                    Console.WriteLine("good");
                }
//                else { Bot.DeleteMessageAsync(e.Message.Chat.Id, e.Message.MessageId); }
                
            }

            //TODO: написать команды + направление движения
            if (e.Message.Text == "/up" && e.Message.Chat.Id != e.Message.From.Id)
            {
//                    string s = FormatAnswers.AnswerUp(Act.TryMove(e.Message.From.Id, a.GetLobbyId(e.Message.From.Id)),
//                    e.Message.From.Username);
//                    Bot.SendTextMessageAsync(e.Message.Chat.Id, s, ParseMode.Markdown);
                    Console.WriteLine("good");
                    stroke++;
                    if (stroke > ruls.RulesList[0]) stroke = 1;
            }

            if (e.Message.Text == "/getinfo")
            {
//                NewGames.StartGame();
                Console.WriteLine("good");
            }
        } 
    }
}
