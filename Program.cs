using System;
using MazeGenerator.TeleBot;

namespace MazeGenerator

{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var token = Console.ReadLine();
            var bot = new MazeBot(token);

            Console.ReadLine();
            bot.BotClient.StopReceiving();
        }
    }
}