using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using ConsoleApplication1;

namespace MazeGenerator

{
      class Program
    {
        static void Main(string[] args)
        {
            var bot = new MazeBot();
            Console.ReadLine();
            bot.Bot.StopReceiving();
        }
    }
}
