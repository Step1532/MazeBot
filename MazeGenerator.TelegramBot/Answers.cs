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
            "Опять не смог пройти, стена"
        };
        public static readonly List<string> MoveArs = new List<string>()
        {
            " и нашел вход в Арсенал",
            " и пополнил запасы патронов и бомб"
        };
        public static readonly List<string> MoveHosp = new List<string>()
        {
            " и нашел госпиталь, излечив здоровье"
        };
        public static readonly List<string> MovePlayer = new List<string>()
        {
            " и встретил игрока *{0}*",
            " и обрадовался встрече с *{0}*",
            " и поздоровался с *{0}*"
        };
        public static readonly List<string> MoveChest = new List<string>()
        {
            " и нашел cундук, осталось отнести его к выходу",
            " и раскопал сокровище",
            " и нашел клад"
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
            "Поставил рану *{0}*"
        };
        public static readonly List<string> Help = new List<string>()
        {
            "Здесь будут советы"
        };
        public static readonly List<string> SkipTurn = new List<string>()
        {
            "Ход пропущен",
            "Пропустил от безысходности?"
        };
        public static readonly List<string> ShootKill = new List<string>()
        {
            "{0} убит",
            "убил {0} х_х"
        };
        public static readonly List<string> StabHit = new List<string>()
        {
            "ранил {0} ",
            "поставил рану {0}"
        };
        public static readonly List<string> StabWall = new List<string>()
        {
            "Никого не ранил",
            "В клетке никого небыло"
        };
        public static readonly List<string> StabKill = new List<string>()
        {
            "да прольется кровь {0}",
            "убил х_х {0}"
        };
        public static readonly List<string> EndGame = new List<string>()
        {
            "Ты победил!",
            "УРРА! Ты  победтл"
        };
        public static readonly List<string> AfkPlayer = new List<string>()
        {
            "AfkPlayer",
            "Игра расформарована"
        };
        public static readonly List<string> NoTurn = new List<string>()
        {
            "Не твой ход!",
            "Ход не ваш"
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
            "Подрыв стены невозможен!"
        };
        public static readonly List<string> ResultBombWall = new List<string>()
        {
            "Стена взорвана",
            "стены нет!"
        };
    }
}