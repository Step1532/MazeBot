using MazeGenerator.TeleBot;
using System;

namespace MazeGenerator

{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var bot = new MazeBot();
            Console.ReadLine();
            bot.Bot.StopReceiving();
        }
    }
}