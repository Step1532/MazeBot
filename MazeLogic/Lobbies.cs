using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        public int IsFullLobby()
        {
            ReadLobbyList();
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
        //TODO: добавление новых лобби
        public void CreateNewLobby()
        {
            
        }
    }
}
