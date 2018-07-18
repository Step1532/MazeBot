using MazeGenerator.MazeLogic;

namespace MazeGenerator.TeleBot
{
    public  class LobbyControl
    {
        public string GenerateLink()
        {
            switch (CheckLobby())
            {
                case 1: return "";
                case 2: return "";
                case 3: return "";
                case 4: return "";
                case 5: return "";
                default:return "Sorry";
            }
        }
        private int CheckLobby()
        {
            int lobbyId = 0;
            var a = new Lobbies();
            var lobbies = a.IsFullLobby();
            int i = 0;
            for (; i < lobbies.Count; i++) if(!lobbies[i]) break;
            //TODO:  в JSON пишем +1
            return i;
        }

    }
}
