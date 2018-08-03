using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MazeGenerator.Core.Services;
using MazeGenerator.Database;
using MazeGenerator.Models.Enums;
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
                return new MessageConfig()
                {
                    Answer = "Вы уже находитесь в лобби",
                    CurrentPlayerId = playerId,
                };
            }

            LobbyService.AddUser(playerId);
            if (LobbyService.EmptyPlaceCount(playerId) != 0)
            {
                return new MessageConfig()
                {
                    Answer =
                        $"Вы добавлены в лобби, осталось игроков для начала игры{LobbyService.EmptyPlaceCount(playerId)}",
                    CurrentPlayerId = playerId,
                };
            }
            BotService.StartGame(playerId);
            msg.AnswerForOther = "Игра начата";
            msg.KeyBoardId = KeyBoardEnum.Move;
            msg.OtherPlayersId = members.ReadMemberList(members.ReadLobbyId(playerId))
                .Select(e => e.UserId)
                .ToList();
            CharacterRepository characterRepository = new CharacterRepository();
            foreach (var item in msg.OtherPlayersId)
            {
                var character = characterRepository.Read(item);
                character.State = CharacterState.InGame;
                characterRepository.Update(character);
            }

            msg.NextPlayerId = members.ReadMemberList(members.ReadLobbyId(playerId)).First().UserId;
            return msg;
        }
    }
}
