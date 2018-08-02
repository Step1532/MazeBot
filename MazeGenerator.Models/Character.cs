using System;
using System.Collections.Generic;
using System.Text;
using MazeGenerator.Models.Enums;

namespace MazeGenerator.Models
{
    public class Character
    {
        public int TelegramUserId { get; set; }
        public string CharacterName { get; set; }
        public int Experience { get; set; }
        public CharacterState State { get; set; }

        public Character()
        {
            
        }
    }
}
