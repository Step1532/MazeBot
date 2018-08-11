using System;
using System.Collections.Generic;
using System.Text;

namespace MazeGenerator.TelegramBot
{
    class AnswersForOther
    {
        public static readonly List<string> MoveGo = new List<string>()
        {
            "{0} прошел {1}",
            "{0} go {1}"
        };
        public static readonly List<string> UndefinedCommand = new List<string>()
        {
            "неизвестная команда",
            "что ты написал?"
        };
        public static readonly List<string> MoveWall = new List<string>()
        {
            "wall {0} {1}",
            "стена {0} {1}"
        };
        public static readonly List<string> MoveArs = new List<string>()
        {
            "Арсенаал {0} {1}",
            "arsenal {0} {1}"
        };
        public static readonly List<string> MoveHosp = new List<string>()
        {
            "hospital {0} {1}",
            "госпиталь {0} {1}"
        };
        public static readonly List<string> MovePlayer = new List<string>()
        {
            "{0} Встретил игрока {1}",
            "{0} Player {1}"
        };
        public static readonly List<string> MoveChest = new List<string>()
        {
            "cундук {0}",
            "сокров ище {0}"
        };
        public static readonly List<string> ExitFalseChest = new List<string>()
        {
            "хреновое сокровище {0}",
            "ложное {0}"
        };
        public static readonly List<string> ShootWall = new List<string>()
        {
            "увы {0}",
            "мимо {0}"
        };
        public static readonly List<string> ShootHit = new List<string>()
        {
            "ранил{0} ",
            "поставил рану {0}"
        };
        public static readonly List<string> SkipTurn = new List<string>()
        {
            "Ходпропущен {0}",
            "Ход пропущен {0}"
        };
        public static readonly List<string> ShootKill = new List<string>()
        {
            "да прольется кровь невинных {0}",
            "убил х_х {0}"
        };
        public static readonly List<string> StabHit = new List<string>()
        {
            "ранил{0} ",
            "поставил рану {0}"
        };
        public static readonly List<string> StabWall = new List<string>()
        {
            "Ходпропущен {0}",
            "Ход пропущен {0}"
        };
        public static readonly List<string> StabKill = new List<string>()
        {
            "да прольется кровь невинных {0}",
            "убил х_х {0}"
        };
        public static readonly List<string> EndGame = new List<string>()
        {
            "ПППООБЕДДил! {0}",
            "УРРА! {0}"
        };
        public static readonly List<string> AfkPlayer = new List<string>()
        {
            "AfkPlayer {0}",
            "Игра расформарована {0}"
        };
        public static readonly List<string> NoTurn = new List<string>()
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
            "Взрыв пустоты?{0}",
            "пусто{0}"
        };
        public static readonly List<string> ResultBombNoBomb = new List<string>()
        {
            "Нет бомб!",
            "Подрыв стены невозможна!"
        };
        public static readonly List<string> ResultBombWall = new List<string>()
        {
            "Стена взорвана{0}",
            "стены нет!{0}"
        };
    }

}

