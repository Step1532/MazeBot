using MazeGenerator.MazeLogic;

namespace MazeGenerator.TeleBot
{
    public  class LobbyControl
    {
        public string GenerateLink()
        {
            //TODO: ссылки на лобби, сделать лобби
            switch (CheckLobby())
            {
                case 1: return "https://t.me/joinchat/EoabPk5DPTqJ231LveIF0g";
                case 2: return "https://ссыль_на-2-лобби";
                case 3: return "https://ссыль-на-3--обби";
                case 4: return "https://ссыль-на-4-лобби";
                case 5: return "https://ссыль-на-5--обби";
                default:return "Sorry, all lobbies are full";
            }
        }
        private int CheckLobby()
        {
            int lobbyId = 0;
            var a = new Lobbies();
            var lobbies = a.IsFullLobby();
            int i = 0;
            for (; i < lobbies.Count; i++) if(!lobbies[i]) break;      
            return i+1;
        }

    }
}
