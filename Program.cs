using MazeGenerator.TeleBot;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MazeGenerator

{
    internal class Program
    {

        private static void Main(string[] args)
        {

            string token;
            token = Console.ReadLine();
            var bot = new MazeBot(token);
            Console.ReadLine();
            bot.Bot.StopReceiving();
        }
    }
}