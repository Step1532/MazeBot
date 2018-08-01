using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using MazeGenerator.Core.Tools;
using MazeGenerator.Database;
using MazeGenerator.Models;
using MazeGenerator.Models.Enums;
using Newtonsoft.Json;

namespace MazeGenerator.TelegramBot
{
    public class LobbyControl
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
                return 1;
            }
            else
            {
                lastuser = players.Last();
                var users = repo.ReadLobbyAll().Where(e => e.LobbyId == lastuser.LobbyId);
                //TODO: чет не понял где добавление игрков
                //TODO: <3
                return 1-users.Count();

            } 

        }
    }
}