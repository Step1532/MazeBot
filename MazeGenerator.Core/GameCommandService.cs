using System;
using System.Collections.Generic;
using System.Text;
using MazeGenerator.Core.Services;
using MazeGenerator.Database;
using MazeGenerator.Models;
using MazeGenerator.Models.Enums;

namespace MazeGenerator.Core
{
    public static class GameCommandService
    {
        public static readonly LobbyRepository LobbyRepository = new LobbyRepository();
        public static readonly MemberRepository MemberRepository = new MemberRepository();
        public static readonly CharacterRepository characters = new CharacterRepository();

        public static ShootResult ShootCommand(int userId, Direction direction)
        {
            Lobby lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(userId));
            lobby.TimeLastMsg = DateTime.Now;

            var shootResult = PlayerLogic.TryShoot(lobby, lobby.Players[lobby.CurrentTurn], direction);

            LobbyRepository.Update(lobby);

            return shootResult;
        }
    }
}
