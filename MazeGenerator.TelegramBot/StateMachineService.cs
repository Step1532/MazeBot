using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MazeGenerator.Core.Services;
using MazeGenerator.Database;
using MazeGenerator.TelegramBot.Models;
using Telegram.Bot.Types.Enums;

namespace MazeGenerator.TelegramBot
{
    public static class StateMachineService
    {
        public static MessageConfig FindGameCommand(int playerId)
        {
            MemberRepository members = new MemberRepository();
            MessageConfig msg = new MessageConfig();
            if (LobbyService.CheckLobby(playerId))
            {
                msg.Answer = "Вы уже находитесь в лобби";
                msg.CurrentPlayerId = playerId;
            }
            else
            {
                LobbyService.AddUser(playerId);
                if (LobbyService.EmptyPlaceCount(playerId) == 0)
                {
                    BotService.StartGame(playerId);
                    msg.AnswerForOther = "Игра начата";
                    msg.KeyBoardId = KeyBoardEnum.Move;
                    msg.OtherPlayersId = members.ReadMemberList(members.ReadLobbyId(playerId))
                        .Select(e => e.UserId)
                        .ToList();
                    msg.NextPlayerId = members.ReadMemberList(members.ReadLobbyId(playerId)).First().UserId;
                }
                else
                {
                    msg.Answer = $"Вы добавлены в лобби, осталось игроков для начала игры{LobbyService.EmptyPlaceCount(playerId)}";
                    msg.CurrentPlayerId = playerId;
                }
            }
            return msg;
        }
    }
}
