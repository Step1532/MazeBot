using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using MazeGenerator.Core.Services;
using MazeGenerator.Core.Tools;
using MazeGenerator.Database;
using MazeGenerator.Models;
using MazeGenerator.Models.Enums;
using MazeGenerator.TelegramBot.Models;

namespace MazeGenerator.TelegramBot
{
    public static class StateMachineService
    {
        public static List<MessageConfig> FindGameCommand(int playerId)
        {
            var members = new MemberRepository();
            var msg = new List<MessageConfig>();
            if (LobbyService.CheckLobby(playerId))
            {
                msg.Add(new MessageConfig
                {
                    Answer = "Вы уже находитесь в лобби",
                    PlayerId = playerId
                });
                return msg;
            }
            LobbyService.AddUser(playerId);
            if (LobbyService.EmptyPlaceCount(playerId) != 0)
            {
                msg.Add(new MessageConfig
                {
                    Answer = $"Вы добавлены в лобби, осталось игроков для начала игры{LobbyService.EmptyPlaceCount(playerId)}",
                    PlayerId = playerId
                });
                return msg;
            }
            LobbyService.StartNewLobby(playerId);
            var memberlist = members.ReadMemberList(members.ReadLobbyId(playerId));

            for (int i = 0; i < memberlist.Count; i++)
            {
                msg.Add(new MessageConfig
                {
                    Answer = "Игра начата",
                    PlayerId = memberlist[i].UserId,
                    KeyBoardId = KeybordConfiguration.WithoutBombAndShootKeyboard()
                });
            }
            var characterRepository = new CharacterRepository();
            foreach (var item in memberlist.Select(e => e.UserId))
            {
                var character = characterRepository.Read(item);
                character.State = CharacterState.InGame;
                characterRepository.Update(character);
            }

            msg.Find(e => e.PlayerId == members.ReadMemberList(members.ReadLobbyId(playerId)).First().UserId).Answer += "Ваш ход";
            return msg;
        }
    }
}