using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public static void AddUser(int lobbyId,int userId)
        {
            MemberRepository repo = new MemberRepository();
            var users = repo.ReadLobbyAll();
            repo.Create(lobbyId, userId);
        }

        public static int EmptyPlaceCount(int userId)
        {
            MemberRepository repo = new MemberRepository();
            var lastuser = repo.ReadLobbyAll().Last();
            var users = repo.ReadLobbyAll().Where(e => e.LobbyId == lastuser.LobbyId);

            //TODO: <3-
            return 3-users.Count();
        }
    }
}