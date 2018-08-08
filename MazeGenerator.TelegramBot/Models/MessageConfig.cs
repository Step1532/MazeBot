using MazeGenerator.Models.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MazeGenerator.TelegramBot.Models
{
    public class MessageConfig
    {
        public string Answer { get; set; }
        public int PlayerId { get; set; }
        public KeyboardType KeyBoardId { get; set; }
    }
}