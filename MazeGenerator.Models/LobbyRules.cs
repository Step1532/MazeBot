using System;

namespace MazeGenerator.Models
{
    public class LobbyRules
    {
        public int ArsenalCount;
        public int ExitCount;
        public int FalseGoldCount;
        public int HolesCount;
        public int HospitalCount;
        public int PlayerMaxHealth;
        public int PlayerMaxGuns;
        public int PlayerMaxBombs;
        public int PlayersCount;
        public Coordinate Size;
        public TimeSpan BanTime;

        public static LobbyRules GenerateTemplateRules()
        {
            return new LobbyRules
            {
                Size = new Coordinate(3, 3),
                ExitCount = 1,
                ArsenalCount = 1,
                HospitalCount = 1,
                HolesCount = 1,
                FalseGoldCount = 1,
                PlayerMaxHealth = 3,
                PlayerMaxGuns = 2,
                PlayerMaxBombs = 3,
                BanTime = TimeSpan.FromDays(1),
                PlayersCount = 1,


            };
        }
        public static LobbyRules Test()
        {
            return new LobbyRules
            {
                Size = new Coordinate(2, 2),
                ExitCount = 0,
                ArsenalCount = 0,
                HospitalCount = 0,
                FalseGoldCount = 0,
                PlayerMaxHealth = 3,
                PlayerMaxGuns = 2,
                PlayerMaxBombs = 3,
                BanTime = TimeSpan.FromDays(1),
            };
        }
    }
}