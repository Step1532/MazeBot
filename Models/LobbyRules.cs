﻿namespace MazeGenerator.Models
{
    public class LobbyRules
    {
        public int ArsenalCount;
        public int ExitCount;
        public int FalseGoldCount;
        public int HolesCount;
        public int HospitalCount;
        public Coordinate Size;

        public static LobbyRules GenerateTemplateRules()
        {
            return new LobbyRules
            {
                Size = new Coordinate(5,5),
                ExitCount = 1,
                ArsenalCount = 1,
                HospitalCount = 1,
                HolesCount = 1,
                FalseGoldCount = 1
            };
        }
    }
}