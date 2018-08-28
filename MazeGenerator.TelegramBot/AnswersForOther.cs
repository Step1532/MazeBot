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
            "{0} смог пройти {1}"
        };
        public static readonly List<string> MoveWall = new List<string>()
        {
            "{0} пошел {1} и уперся в стену ",
        };
        public static readonly List<string> MovePlayer = new List<string>()
        {
            "{0} прошел {2} и встретил игрока {1}",
        };
        public static readonly List<string> MoveChest = new List<string>()
        {
            "{0} прошел {1} и нашел сундук",
        };
        public static readonly List<string> ShootWall = new List<string>()
        {
            "{0} стреляет {1} и не попадает",
        };
        public static readonly List<string> ShootHit = new List<string>()
        {
            "{0} стреляет {2} и ставит рану {1}",
        };
        public static readonly List<string> SkipTurn = new List<string>()
        {
            "{0} пропустил ход",
        };
        public static readonly List<string> ShootKill = new List<string>()
        {
            "{0} стреляет {2} и убивает {1}",
        };
        public static readonly List<string> StabHit = new List<string>()
        {
            "{0} бьет кинжалом и ставит рану {1}",
        };
        public static readonly List<string> StabWall = new List<string>()
        {
            "{0} бьет кинжалом и никого не задевает",
        };
        public static readonly List<string> StabKill = new List<string>()
        {
            "{0} бьет кинжалом и ставит убивает {1}",
        };
        public static readonly List<string> EndGame = new List<string>()
        {
            "Победил {0}, не сдавайся, твоя победа еще впереди",
        };
        public static readonly List<string> NotBullet = new List<string>()
        {
            "{0} стреляет {1}, но у него  нет пуль",
        };
        public static readonly List<string> ResultBombVoid = new List<string>()
        {
            "{0} кидает бомбу {1}, но стены там нет",
        };
        public static readonly List<string> ResultBombWall = new List<string>()
        {
            "{0} кидает бомбу {1}, и взрывает стену",
        };
    }

}

