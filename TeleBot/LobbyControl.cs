using System.Collections.Generic;
using System.IO;
using MazeGenerator.MazeLogic;
using MazeGenerator.Models;
using MazeGenerator.NewGame;
using MazeGenerator.Tools;
using Newtonsoft.Json;

namespace MazeGenerator.TeleBot
{
    public  class LobbyControl
    {
        public string GenerateLink(int userId)
        {
            if (Validation(userId))
            {
                //TODO: ссылки на лобби, сделать лобби
                switch (CheckLobby())
                {
                    case 1: return "https://t.me/joinchat/EoabPk5DPTqJ231LveIF0g";
                    case 2: return "https://ссыль_на-2-лобби";
                    case 3: return "https://ссыль-на-3--обби";
                    case 4: return "https://ссыль-на-4-лобби";
                    case 5: return "https://ссыль-на-5--обби";
                    default: return null;
                    //default: return "Sorry, all lobbies are full";
                }
            }

            return null;
        }
        //TODO: слздать фал users
        public bool Validation(int userId)
        {
            return true;
            List<int> users = JsonConvert.DeserializeObject<List<int>>(File.ReadAllText($@"\users.json"));
            
            //for(int)

        }
        //TODO: GetLobbyId(long chatId)
        public int CheckLobbyId(long lobbyid)
        {
            switch (lobbyid)
            {
                case 310811454: return 1;
                case 2: return 1;
                case 3: return 2;
                case 4: return 3;
                case 5: return 4;
                default: return 1;
            }
        }
        //TODO: rename FindEmptyLobby
        private int CheckLobby()
        {
            ParseJsonManager e = new ParseJsonManager();
            List<int> LobbyList = e.GetLobbiesList();
            Rules a = new Rules();
            for (int i = 0; i < LobbyList.Count; i++)
            {
                if (LobbyList[i] != a.RulesList[0])
                {
                    return i + 1;
                }
            }
            return 0;
        }

        public void AddPlayer(Player player, int lobbyId)
        {
            JsonManager.UpdateJson($@"\users.json", (List<int> users) => { users.Add(player.Playerid); });
            JsonManager.UpdateJson("lobbiesPlayerCount.json", (List<int> lobbiesList) => { lobbiesList[lobbyId]++; });
            Lobby lobby = new Lobby(lobbyId);
            lobby.Load();
            lobby.Players.Add(player);
            player.UserCoordinate = lobby.Maze.GenerateRandomPosition();
            lobby.Save();
        }
    }
}
