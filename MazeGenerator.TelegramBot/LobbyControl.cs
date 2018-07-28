using System.Collections.Generic;
using System.IO;
using MazeGenerator.Core.Tools;
using MazeGenerator.Database;
using MazeGenerator.Models;
using MazeGenerator.Models.Enums;
using Newtonsoft.Json;

namespace MazeGenerator.TelegramBot
{
    public class LobbyControl
    {
        public string GenerateLink(int userId)
        {
            if (Validation(userId))
            {
                return "https://t.me/joinchat/EoabPk5DPTqJ231LveIF0g";
            }

            //if (Validation(userId))
            //{
            //    //TODO: ссылки на лобби, сделать лобби
            //     switch (CheckLobby())
            //    {
            //        case 1: return "https://t.me/joinchat/EoabPk5DPTqJ231LveIF0g";
            //        case 2: return "https://ссыль_на-2-лобби";
            //        case 3: return "https://ссыль-на-3--обби";
            //        case 4: return "https://ссыль-на-4-лобби";
            //        case 5: return "https://ссыль-на-5--обби";
            //        default: return null;
            //    }
            //}

            return null;
        }

        //TODO: создать файл users
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