using System;
using System.Linq;
using MazeGenerator.Database;
using MazeGenerator.Models;
using MazeGenerator.Models.Enums    ;
using MazeGenerator.Core.GameGenerator;

namespace MazeGenerator.Core.Services
{
    public static class LobbyService
    {
        private static MemberRepository _memberRepository = new MemberRepository();
        private static LobbyRepository _lobbyRepository = new LobbyRepository();

        public static void StartNewLobby(int playerId)
        {
            var characters = new CharacterRepository();
            var character = characters.Read(playerId);
            var gameid = _memberRepository.ReadLobbyId(playerId);
            var players = _memberRepository.ReadMemberList(gameid);
            var lobby = new Lobby(gameid);
            foreach (var p in players)
            {
                var player = new Player
                {
                    Rotate = Direction.North,
                    Health = lobby.Rules.PlayerMaxHealth,
                    TelegramUserId = p.UserId,
                    HeroName = character.CharacterName
                };
                lobby.Players.Add(player);
            }

            LobbyGenerator.InitializeLobby(lobby);
            _lobbyRepository.Create(lobby);
        }

        public static bool CheckLobby(int userId)
        {
            var users = _memberRepository.ReadLobbyAll();
            return users.Any(e => e.UserId == userId);
        }
        //TODO:
        public static void AddUser(int userId)
        {
            var members = _memberRepository.ReadLobbyAll();
            if (members.Count == 0)
            {
                _memberRepository.Create(1, userId);
                return;
            }
            var  member =  members.Last();
            if (EmptyPlaceCount(member.UserId) == 0)
            {
                _memberRepository.Create(member.LobbyId+1, userId);
            }
            else
            {
                _memberRepository.Create(member.LobbyId, userId);
            }
        }

        public static int EmptyPlaceCount(int userId)
        {
            var players = _memberRepository.ReadLobbyAll();
            Member lastuser;
            if (players.Count == 0)
            {
                return LobbyRules.GenerateTemplateRules().PlayersCount;
            }
            else
            {
                lastuser = players.Last();
                var users = _memberRepository.ReadLobbyAll().Where(e => e.LobbyId == lastuser.LobbyId);
                return LobbyRules.GenerateTemplateRules().PlayersCount-users.Count();

            } 

        }

        public static void EndTurn(Lobby lobby)
        {
            lobby.CurrentTurn = (lobby.CurrentTurn + 1) % lobby.Players.Count;
            lobby.TimeLastMsg = DateTime.Now;
            _lobbyRepository.Update(lobby);
        }

        public static bool CanMakeTurn(Lobby lobby, int userId)
        {
            return lobby.Players.FindIndex(e => e.TelegramUserId == userId) == lobby.CurrentTurn;
        }
    }
}