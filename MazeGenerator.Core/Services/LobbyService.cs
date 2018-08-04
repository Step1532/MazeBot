using System.Linq;
using MazeGenerator.Database;
using MazeGenerator.Models;
using MazeGenerator.Models.Enums    ;
using MazeGenerator.Core.GameGenerator;

namespace MazeGenerator.Core.Services
{
    public static class LobbyService
    {
        private static MemberRepository repo = new MemberRepository();


        public static void StartNewLobby(int playerId)
        {
            var characters = new CharacterRepository();
            var character = characters.Read(playerId);
            var gameid = repo.ReadLobbyId(playerId);
            var players = repo.ReadMemberList(gameid);
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
            var repository = new LobbyRepository();
            repository.Create(lobby);
        }

        public static bool CheckLobby(int userId)
        {
            var users = repo.ReadLobbyAll();
            return users.Any(e => e.UserId == userId);
        }

        public static void AddUser(int userId)
        {
            var members = repo.ReadLobbyAll();
            if (members.Count == 0)
            {
                repo.Create(1, userId);
                return;
            }
            var  member =  members.Last();
            if (EmptyPlaceCount(member.UserId) == 0)
            {
                repo.Create(member.LobbyId+1, userId);
            }
            else
            {
                repo.Create(member.LobbyId, userId);
            }
        }

        public static int EmptyPlaceCount(int userId)
        {
            var players = repo.ReadLobbyAll();
            Member lastuser;
            if (players.Count == 0)
            {
                //TODO: Это что?
                return 1;
            }
            else
            {
                lastuser = players.Last();
                var users = repo.ReadLobbyAll().Where(e => e.LobbyId == lastuser.LobbyId);
                //TODO: <3
                return 1-users.Count();

            } 

        }
    }
}