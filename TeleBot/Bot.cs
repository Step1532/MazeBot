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
                    var a = new LobbyControl();
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, a.GenerateLink(), ParseMode.Markdown);
                    Console.WriteLine("good");

                }
            }
            
            //TODO: написать команды + направление движения
            if (e.Message.Text == "/up")
            {
                if (e.Message.Chat.Id == e.Message.From.Id)
                {
                    var a = new LobbyControl();
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, a.GenerateLink(), ParseMode.Markdown);
                    Console.WriteLine("good");

                }
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
                bool[,] maze = a.GetMazeMap(1);
                Bot.SendTextMessageAsync(e.Message.Chat.Id, "```" + maze + "```", ParseMode.Markdown);
                Console.WriteLine("good");
            }
        } 
    }
}
