using System;
using MazeGenerator.GameGenerator;
using MazeGenerator.MazeLogic;
using MazeGenerator.TeleBot;

namespace MazeGenerator

{
    internal class Program
    {
        private static void Main(string[] args)
        {
           //var token = Console.ReadLine();
           //var bot = new MazeBot(token);
            Lobby lobby = new Lobby(1);
            LobbyGenerator.GenerateLobby(lobby);
            FormatAnswers.ConsoleApp(lobby.Maze);
            Console.ReadLine();
           //bot.BotClient.StopReceiving();
        }
    }
}