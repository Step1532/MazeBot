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
        public ParseJsonManager PJson = new ParseJsonManager();
        public  JsonManager MJson = new JsonManager();
        public Rules ruls = new Rules();
        public MazeLogic.MazeLogic Act = new MazeLogic.MazeLogic();
        public int stroke = 1;

        public MazeBot(string _tMaze)
        {
            Bot = new TelegramBotClient(_tMaze);
            Bot.OnMessage += OnNewMessage;
            Bot.StartReceiving();
        }

        public void OnNewMessage(object sender, MessageEventArgs e)
        {

            if (e.Message.Type != MessageType.Text) return;
            if (e.Message.Text == "/start")
            {
                if (e.Message.Chat.Id == e.Message.From.Id)
                {
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, a.GenerateLink(), ParseMode.Markdown);
                    Console.WriteLine("good");

                }
            }
            if (e.Message.Text == "/add")
            {

                    int lobbydId = a.CheckLobbyId(e.Message.Chat.Id);
                    var lobbyList = PJson.GetLobbiesList();
                //TODO переделать, ограничение, разбан чата
                if (lobbyList[lobbydId - 1] != ruls.RulesList[0]) 
                {
                    lobbyList[lobbydId - 1]++;
                    player.AddNewPlayer(lobbyList[lobbydId - 1], e.Message.From.Id, lobbydId);
                    NewGames game = new NewGames();
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, game.CheckStartGame(lobbyList[lobbydId - 1], lobbydId),
                        ParseMode.Markdown);
                    MJson.WriteLobbiesPlayerCountToJson(lobbyList);
                    Console.WriteLine("good");
                }
                else { Bot.DeleteMessageAsync(e.Message.Chat.Id, e.Message.MessageId); }
                
            }

            //TODO: написать команды + направление движения
            if (e.Message.Text == "/up" && e.Message.Chat.Id != e.Message.From.Id)
            {
                    string s = FormatAnswers.AnswerUp(Act.TryMoveUp(e.Message.From.Id, a.CheckLobbyId(e.Message.From.Id)),
                    e.Message.From.Username);
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, s, ParseMode.Markdown);
                    Console.WriteLine("good");
                    stroke++;
                    if (stroke > ruls.RulesList[0]) stroke = 1;
            }
            if (e.Message.Text == "/down")
            {
                if (e.Message.Chat.Id == e.Message.From.Id)
                {
                    var a = new LobbyControl();
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, a.GenerateLink(), ParseMode.Markdown);
                    Console.WriteLine("good");

                }
            }
            if (e.Message.Text == "/left")
            {
                if (e.Message.Chat.Id == e.Message.From.Id)
                {
                    var a = new LobbyControl();
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, a.GenerateLink(), ParseMode.Markdown);
                    Console.WriteLine("good");

                }
            }
            if (e.Message.Text == "/right")
            {
                if (e.Message.Chat.Id == e.Message.From.Id)
                {
                    var a = new LobbyControl();
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, a.GenerateLink(), ParseMode.Markdown);
                    Console.WriteLine("good");

                }
            }
            if (e.Message.Text == "/fire")
            {
                if (e.Message.Chat.Id == e.Message.From.Id)
                {
                    var a = new LobbyControl();
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, a.GenerateLink(), ParseMode.Markdown);
                    Console.WriteLine("good");

                }
            }
            if (e.Message.Text == "/right")
            {
                if (e.Message.Chat.Id == e.Message.From.Id)
                {
                    var a = new LobbyControl();
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, a.GenerateLink(), ParseMode.Markdown);
                    Console.WriteLine("good");

                }
            }

            if (e.Message.Text == "/getinfo")
            {
                NewMaze.GetNewMaze(1);
                ParseJsonManager a = new ParseJsonManager();
                Console.WriteLine("good");
            }
        } 
    }
}
