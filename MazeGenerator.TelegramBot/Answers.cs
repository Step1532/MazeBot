using System.Collections.Generic;
using MazeGenerator.Models;

namespace MazeGenerator.TelegramBot
{
    public class Answers
    {
        public static readonly List<string> MoveGo = new List<string>()
        {
            "прошел",
            "go"
        };
        public static readonly List<string> MoveWall = new List<string>()
        {
            "wall",
            "стена"
        };
        public static readonly List<string> MoveArs = new List<string>()
        {
            "Арсенаал",
            "arsenal"
        };
        public static readonly List<string> MoveHosp = new List<string>()
        {
            "hospital",
            "госпиталь"
        };
        public static readonly List<string> MovePlayer = new List<string>()
        {
            "{0}Встретил игрока{1}",
            "Player"
        };
        public static readonly List<string> MoveChest = new List<string>()
        {
            "cундук",
            "сокров ище"
        };
        public static readonly List<string> ExitFalseChest = new List<string>()
        {
            "хреновое сокровище",
            "ложное"
        };
        public static readonly List<string> ShootWall = new List<string>()
        {
            "увы",
            "мимо"
        };
        public static readonly List<string> ShootHit = new List<string>()
        {
            "ранил",
            "поставил рану"
        };
        public static readonly List<string> SkipTurn = new List<string>()
        {
            "Ходпропущен",
            "Ход пропущен"
        };
        public static readonly List<string> ShootKill = new List<string>()
        {
            "да прольется кровь невинных",
            "убил х_х"
        };
        public static readonly List<string> EndGame = new List<string>()
        {
            "ПППООБЕДДил!",
            "УРРА!"
        }; public static readonly List<string> NoTurn = new List<string>()
        {
            "Не твой ход!",
            "ход не ваш"
        };
        public static readonly List<string> NotBullet = new List<string>()
        {
            "Нет пуль!",
            "Стрельба невозможна!"
        };
        public static readonly List<string> ResultBombVoid = new List<string>()
        {
            "Взрыв пустоты?",
            "пусто"
        };
        public static readonly List<string> ResultBombNoBomb = new List<string>()
        {
            "Нет бомб!",
            "Подрыв стены невозможна!"
        };
        public static readonly List<string> ResultBombWall = new List<string>()
        {
            "Стена взорвана",
            "стены нет!"
        };
    }
}