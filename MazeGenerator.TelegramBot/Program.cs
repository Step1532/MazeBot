using System;
using System.Collections.Generic;
using System.IO;
using MazeGenerator.Core.GameGenerator;
using MazeGenerator.Core.Services;
using MazeGenerator.Core.Tools;
using MazeGenerator.Models;
using MazeGenerator.Models.Enums;
using Newtonsoft.Json;

namespace MazeGenerator.TelegramBot
{
    class Program
    {

        private static void Main(string[] args)
        {
            var token = Console.ReadLine();
            File.WriteAllText(@"usersinLobby.json", JsonConvert.SerializeObject(new List<Member>()));
            var bot = new MazeBot(token);
            Console.ReadLine();
            bot.BotClient.StopReceiving();
        }
    }
}
