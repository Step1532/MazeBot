using System.Collections.Generic;
using MazeGenerator.Models.Enums;

namespace MazeGenerator.Models
{
    public class MessageConfig
    {
        public string Answer { get; set; }
        public string AnswerForOther { get; set; }
        public int CurrentPlayerId { get; set; }
        public int NextPlayerId { get; set; }
        public List<int> OtherPlayersId { get; set; }
        public KeyboardType KeyBoardId { get; set; }
    }
}