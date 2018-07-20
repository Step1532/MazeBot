using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenerator.Models
{
    public class LobbyRules
    {
        public Coordinate Size;
        public int ExitCount;
        public int ArsenalCount;
        public int HospitalCount;
        public int HolesCount;
        public int FalseGoldCount;

        public static LobbyRules GenerateTemplateRules()
        {
            return new LobbyRules
            {
                Size = new Coordinate(10, 10),
                ExitCount = 1,
                ArsenalCount = 1,
                HospitalCount = 1,
                HolesCount = 1,
                FalseGoldCount = 1
            };
        }
    }
}
