using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Telegram.Bot;
using System.Threading.Tasks;
using ConsoleApplication1;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
namespace MazeGenerator
{
    public class MazeBot
    {
        private static readonly string _tMaze = "557358914:AAE03Faw9-BwKFygJHFMl530FiGH9sPvB6Y";
        public readonly TelegramBotClient Bot;

        public MazeBot()
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
            if (e.Message.Text == "/getinfo")
            {
                var maze = NewMaze.GetNewMaze();
                Bot.SendTextMessageAsync(e.Message.Chat.Id, "```" + maze + "```", ParseMode.Markdown);
                Console.WriteLine("good");
            }
        } 
    }
}
