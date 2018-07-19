using MazeGenerator.TeleBot;
using System;

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