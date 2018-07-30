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
        public static int EmptyPlaceCount(int userId)
        {
            MemberRepository repo = new MemberRepository();
            var lastuser = repo.ReadLobbyAll().Last();
            var users = repo.ReadLobbyAll().Where(e => e.LobbyId == lastuser.LobbyId);
            return 4-users.Count();
        }
        public bool Validation(int userId)
        {
            return true;
            var users = JsonConvert.DeserializeObject<List<int>>(File.ReadAllText($@"\users.json"));
            //TODO: чет сделать  с этим методом
            //TODO: возможно стоит дать ссылкуу на лобби в котором он учавствует
            //            if (users.TrueForAll(e => userId))
            {
                //                JsonManager.UpdateJson("onlineUsersId.json", (List<int> users) => { users.Add(userId); });   
            }
        }
        public static int GetLobbyId(long chatid)
        {
            switch (chatid)
            {
                case 310811454: return 1;
                case 2: return 1;
                case 3: return 2;
                case 4: return 3;
                case 5: return 4;
                default: return 1;
            }
        }

        public void AddPlayer(int playerId, int lobbyId)
        {
            JsonManager.UpdateJson(@"\users.json", (List<int> users) => { users.Add(playerId); });
            JsonManager.UpdateJson("lobbiesPlayerCount.json", (List<int> lobbiesList) => { lobbiesList[lobbyId]++; });

            LobbyRepository lobbyRepository = new LobbyRepository();
            var lobby = lobbyRepository.Read(lobbyId);

            var player = new Player
            {
                PlayerId = playerId,
                Rotate = Direction.North,
                UserCoordinate = lobby.Maze.GenerateRandomPosition(),
            };
            lobby.Players.Add(player);
            lobbyRepository.Update(lobby);
        }

        private int FindEmptyLobby()
        {
            //            List<int> LobbyList = e.GetLobbiesList();
            //            for (int i = 0; i < LobbyList.Count; i++)
            {
                //                if (LobbyList[i] != a.RulesList[0])
                {
                    //                    return i + 1;
                }
            }
            return 0;
        }
    }
}