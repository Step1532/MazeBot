using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGenerator.Tools;

namespace MazeGenerator.MazeLogic
{
    public class Lobbies
    {
        public List<int> LobbyList = new List<int>();

        public void ReadLobbyList()
        {
            ParseJsonManager e = new ParseJsonManager();
            LobbyList = e.GetLobbiesList(); 
        }
        public List<bool> IsFullLobby()
        {
            ReadLobbyList();
            Rules a = new Rules();
            List<bool> lobbyList = new List<bool>();
            for (int i = 0; i < LobbyList.Count; i++)
            {
                if (LobbyList[i] == a.RulesList[0]) lobbyList.Add(true);
            }
            return lobbyList;
        }
        //TODO: добавление новых лобби
        public void CreateNewLobby()
        {
            
        }
    }
}
