using System.Collections.Generic;
using MazeGenerator.Models;

namespace MazeGenerator.TelegramBot
{
    public class Answers
    {
        public readonly List<string> AnswerId0 = new List<string>()
        {
            "прошел",
            "go"
        };
        //public List<string> AnswerId1 = new List<string>()
        //{
        //    "wall",
        //    "стена"
        //};
        //public List<string> AnswerId2 = new List<string>()
        //{
        //    "Арсенаал",
        //    "arsenal"
        //};
        //public List<string> AnswerId3 = new List<string>()
        //{
        //    "попал",
        //    "попадение"
        //};
        //public List<string> AnswerId4 = new List<string>()
        //{
        //    "увы",
        //    "мимо"
        //};
        //public List<string> AnswerId5 = new List<string>()
        //{
        //    "ПППООБЕДДА!",
        //    "УРРА!"
        //};
        public static string GenerateArsenalAnswer(Player player)
        {
            return "Arsenal ";
        }
        public static string GeneratePlayerAnswer(Player player1, Player player2)
        {
            return "Player ";
        }
        public static string GenerateWallAnswer(Player player)
        {
            return "Wall ";
        }
        public static string GenerateHospitalAnswer(Player player)
        {
            return "Hospital ";
        }
        public static string GenerateChestAnswer(Player player)
        {
            return "Chest ";
        }
        public static string GenerateEndAnswer(Player player)
        {
            return "End ";
        }
    }
}