using System.Linq;
using MazeGenerator.Database;
using MazeGenerator.Models;

namespace MazeGenerator.Core.Services
{
	//TODO: Перенести в .Core
    public class LobbyService
    {
        public static bool CheckLobby(int userId)
        {
            MemberRepository repo = new MemberRepository();
            var users = repo.ReadLobbyAll();
            return Enumerable.Any<Member>(users, e => e.UserId == userId);
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
            var  member =  Enumerable.Last<Member>(members);
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
                lastuser = Enumerable.Last<Member>(players);
                var users = Enumerable.Where<Member>(repo.ReadLobbyAll(), e => e.LobbyId == lastuser.LobbyId);
                //TODO: <3
                return 1-users.Count();

            } 

        }
    }
}