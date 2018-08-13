using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;

namespace MazeGenerator.TelegramBot.Models
{
    public static class KeybordConfiguration
    {
        public static InlineKeyboardMarkup ChooseDirectionKeyboard()
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new [] // first row
                {
                    //     InlineKeyboardButton.WithCallbackData(" ", "0"),
                    InlineKeyboardButton.WithCallbackData("⬆️ Вперед", "1"),
                    //     InlineKeyboardButton.WithCallbackData(" ", "0"),
                },
                new [] // second row
                {
                    InlineKeyboardButton.WithCallbackData("⬅️ Влево", "2"),
                    //     InlineKeyboardButton.WithCallbackData(" ", "1"),
                    InlineKeyboardButton.WithCallbackData("Вправо ➡️", "3"),
                },
                new [] // third row
                {
                    //     InlineKeyboardButton.WithCallbackData(" ", "0"),
                    InlineKeyboardButton.WithCallbackData("⬇️ Назад", "4"),
                    //     InlineKeyboardButton.WithCallbackData(" ", "0"),
                },
            });
            return inlineKeyboard;
        }


        public static ReplyKeyboardMarkup WithoutBombAndShootKeyboard()
        {
            var rkm = new ReplyKeyboardMarkup();

            rkm.Keyboard =
                new KeyboardButton[][]
                {
                    CreateButtonList("Вперед"),
                    CreateButtonList("Влево", "Вправо"),
                    CreateButtonList("Пропуск хода", "Назад", "Удар кинжалом"),
                };

            rkm.OneTimeKeyboard = true;
          return rkm;
        }

        public static ReplyKeyboardMarkup WithoutBombKeyBoard()
        {
            var rkm = new ReplyKeyboardMarkup();

            rkm.Keyboard =
                new KeyboardButton[][]
                {
                    CreateButtonList("Вперед"),
                    CreateButtonList("Влево", "Выстрел", "Вправо"),
                    CreateButtonList("Пропуск хода", "Назад", "Удар кинжалом"),
                };

            rkm.OneTimeKeyboard = true;
            return rkm;
        }

        public static ReplyKeyboardMarkup WithoutShootKeyBoard()
        {
            var rkm = new ReplyKeyboardMarkup();

            rkm.Keyboard =
                new KeyboardButton[][]
                {
                     CreateButtonList("Вперед"),
                     CreateButtonList("Влево", "Взрыв стены", "Вправо"),
                     CreateButtonList("Пропуск хода", "Назад", "Удар кинжалом"),
                };
            rkm.OneTimeKeyboard = true;
            return rkm;
        }

        public static ReplyKeyboardMarkup NewKeyBoard()
        {
            var rkm = new ReplyKeyboardMarkup();

            rkm.Keyboard =
                new KeyboardButton[][]
                {
                    CreateButtonList("Выстрел", "Вперед", "Взрыв стены"),
                    CreateButtonList("Влево", "Вправо"),
                    CreateButtonList("Пропуск хода", "Назад", "Удар кинжалом"),
                };
            rkm.OneTimeKeyboard = true;
            return rkm;
        }

        private static KeyboardButton[] CreateButtonList(params string[] textList)
        {
            return textList.Select(s => new KeyboardButton(s)).ToArray();
        }
    }
}
