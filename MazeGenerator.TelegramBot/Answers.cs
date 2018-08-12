using System.Collections.Generic;
using MazeGenerator.Models;

namespace MazeGenerator.TelegramBot
{
    public class Answers
    {
        public static readonly List<string> MoveGo = new List<string>()
        {
            "Тебе снова везет, ты смог пройти дальше",
            "Снова прошел",
            "Продвинулся",
            "Немного двинулся",
            "А ты везунчик, прошел",
            "Прошел",
        };
        public static readonly List<string> UndefinedCommand = new List<string>()
        {
            "Неизвестная команда",
            "Такой команды нет",
            "Непонимаю, скажи иначе",
            "Я твоя не понимать",
        };
        public static readonly List<string> MoveWall = new List<string>()
        {
            "Впереди стена. Кто знает, может она Великая...",
            "Ну вот, опять стена",
            "Стена",
        };
        public static readonly List<string> MoveArs = new List<string>()
        {
            " и нашел вход в Арсенал",
            " и пополнил запасы патронов и бомб"
        };
        public static readonly List<string> MoveHosp = new List<string>()
        {
            " и сказал \"Опять медики...\" зайдя в госпиталь",
            " и увидел крест... \"нет, вроде бы не секта\" - сказал ты, излечив здоровье"
        };
        public static readonly List<string> MovePlayer = new List<string>()
        {
            " и встретил игрока *{0}*",
            " и обрадовался встрече с *{0}*"
        };
        public static readonly List<string> MoveChest = new List<string>()
        {
            " и нашел cундук, осталось отнести его к выходу",
            " и раскопал сокровище"
        };
        public static readonly List<string> ExitFalseChest = new List<string>()
        {
            " и стал на выход, что ж, неповезло, хреновое сокровище",
            " и увы это ложный клад"
        };
        public static readonly List<string> ShootWall = new List<string>()
        {
            "Целься точнее",
            "Научись сначала стрелять"
        };
        public static readonly List<string> ShootHit = new List<string>()
        {
            "Ранил *{0}*",
            "Поставил рану *{}*"
        };
        public static readonly List<string> Help = new List<string>()
        {
            "Совет"
        };
        public static readonly List<string> SkipTurn = new List<string>()
        {
            "Ход пропущен",
            "Пропустил от безысходности?"
        };
        public static readonly List<string> ShootKill = new List<string>()
        {
            "да прольется кровь {0}",
            "убил {0} х_х"
        };
        public static readonly List<string> StabHit = new List<string>()
        {
            "ранил {0} ",
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
            "ПППООБЕДДил!",
            "УРРА!"
        };
        public static readonly List<string> AfkPlayer = new List<string>()
        {
            "AfkPlayer",
            "Игра расформарована"
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