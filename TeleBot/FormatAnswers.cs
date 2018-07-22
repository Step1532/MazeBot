using System;

namespace MazeGenerator.TeleBot
{
    public static class FormatAnswers
    {
        public static string AnswerUp(bool isAction, string username)
        {
            return null;
        }

        public static void ConsoleApp(Byte[,] maze)
        {
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    Console.Write(maze[i,j] == 0 ? "  " : "0 ");
                }
                Console.WriteLine();
            }
        }
    } 
}