using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace MazeGenerator.TelegramBot
{
    public static class BotTools
    {
        public static InlineKeyboardMarkup NewInlineKeyBoardForChooseDirection()
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new [] // first row
                {
                    //     InlineKeyboardButton.WithCallbackData(" ", "0"),
                    InlineKeyboardButton.WithCallbackData("⬆️ Вверх", "1"),
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
                    InlineKeyboardButton.WithCallbackData("⬇️ Вниз", "4"),
                    //     InlineKeyboardButton.WithCallbackData(" ", "0"),
                },
            });
            return inlineKeyboard;
        }

        public static ReplyKeyboardMarkup NewKeyBoardWithoutBombAndShoot()
        {
            var rkm = new ReplyKeyboardMarkup();

            rkm.Keyboard =
                new KeyboardButton[][]
                {
                    new KeyboardButton[]
                    {
                        new KeyboardButton("Вверх"),
                    },

                    new KeyboardButton[]
                    {
                        new KeyboardButton("Влево"),
                        new KeyboardButton("Вправо")
                    },

                    new KeyboardButton[]
                    {
                        new KeyboardButton("Пропуск хода"),
                        new KeyboardButton("Вниз"),
                        new KeyboardButton("Удар кинжалом"),
                    }
                };

          return rkm;
        }
        public static ReplyKeyboardMarkup NewKeyBoardWithoutBomb()
        {
            var rkm = new ReplyKeyboardMarkup();

            rkm.Keyboard =
                new KeyboardButton[][]
                {
                    new KeyboardButton[]
                    {
                        new  KeyboardButton("Вверх"),
                    },

                    new KeyboardButton[]
                    {
                        new KeyboardButton("Влево"),
                        new KeyboardButton("Выстрел"),
                        new KeyboardButton("Вправо")
                    },

                    new KeyboardButton[]
                    {
                        new KeyboardButton("Пропуск хода"),
                        new KeyboardButton("Вниз"),
                        new KeyboardButton("Удар кинжалом"),
                    }
                };

            return rkm;
        }
        public static ReplyKeyboardMarkup NewKeyBoardWithoutShoot()
        {
            var rkm = new ReplyKeyboardMarkup();

            rkm.Keyboard =
                new KeyboardButton[][]
                {
                    new KeyboardButton[]
                    {
                        new KeyboardButton("Вверх"),
                    },

                    new KeyboardButton[]
                    {
                        new KeyboardButton("Влево"),
                        new KeyboardButton("Взрыв стены"),
                        new KeyboardButton("Вправо")
                    },

                    new KeyboardButton[]
                    {
                        new KeyboardButton("Пропуск хода"),
                        new KeyboardButton("Вниз"),
                        new KeyboardButton("Удар кинжалом"),
                    }
                };
            return rkm;
        }
        public static ReplyKeyboardMarkup NewKeyBoard()
        {
            var rkm = new ReplyKeyboardMarkup();

            rkm.Keyboard =
                new KeyboardButton[][]
                {
                    new KeyboardButton[]
                    {
                        new KeyboardButton("Выстрел"),
                        new KeyboardButton("Вверх"),
                        new KeyboardButton("Взрыв стены"),
                    },

                    new KeyboardButton[]
                    {
                        new KeyboardButton("Влево"),
                        new KeyboardButton("Вправо")
                    },

                    new KeyboardButton[]
                    {
                        new KeyboardButton("Пропуск хода"),
                        new KeyboardButton("Вниз"),
                        new KeyboardButton("Удар кинжалом"),
                    }
                };
            return rkm;
        }
    }
}
