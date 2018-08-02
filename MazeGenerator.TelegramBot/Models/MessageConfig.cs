using System.Collections.Generic;

namespace MazeGenerator.TelegramBot.Models
{
    public class MessageConfig
    {
        public string Answer { get; set; }
        public string AnswerForOther { get; set; }
        public int CurrentPlayerId { get; set; }
        public int NextPlayerId { get; set; }
        public List<int> OtherPlayersId { get; set; }
        public KeyBoardEnum KeyBoardId { get; set; }
    }
}
