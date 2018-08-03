using System.Linq;
using MazeGenerator.Database;
using MazeGenerator.Models;

namespace MazeGenerator.Core.Services
{
    public class LobbyService
    {
        public static bool CheckLobby(int userId)
        {
            MemberRepository repo = new MemberRepository();
            var users = repo.ReadLobbyAll();
            return users.Any(e => e.UserId == userId);
        }
        public static void AddUser(int userId)
        {
            MemberRepository repo = new MemberRepository();
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
            MemberRepository repo = new MemberRepository();
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