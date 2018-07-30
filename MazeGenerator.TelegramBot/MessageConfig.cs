using System;
using System.Collections.Generic;
using System.Text;

namespace MazeGenerator.TelegramBot
{
    public class MessageConfig
    {
        public string Answer { get; set; }
        public string AnswerForOther { get; set; }
        public KeyBoardEnum KeyBoardId { get; set; }
    }
}
