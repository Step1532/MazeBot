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
        private int CheckLobby()
        {
            var a = new Lobbies();
            return a.IsFullLobby();
        }

    }
}
