﻿using System.Linq;
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


        public static ReplyKeyboardMarkup WithoutBombAndShootKeyboard()
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

        public static ReplyKeyboardMarkup WithoutBombKeyBoard()
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

        public static ReplyKeyboardMarkup WithoutShootKeyBoard()
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
                    CreateButtonList("Выстрел", "Вверх", "Взрыв стены"),
                    CreateButtonList("Влево", "Вправо"),
                    CreateButtonList("Пропуск хода", "Вниз", "Удар кинжалом"),
                };
            return rkm;
        }

        private static KeyboardButton[] CreateButtonList(params string[] textList)
        {
            return textList.Select(s => new KeyboardButton(s)).ToArray();
        }
    }
}