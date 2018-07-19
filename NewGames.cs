using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGenerator.NewGame;
using MazeGenerator.TeleBot;
using MazeGenerator.Tools;

namespace MazeGenerator
{
    class NewGames
    {
        public Rules ruls = new Rules();
        public LobbyControl a = new LobbyControl();
        public Player player = new Player();
        public ParseJsonManager PJson = new ParseJsonManager();
        public JsonManager MJson = new JsonManager();

        public void StartGame(int lobbyid)
        {
            
        }

        public string CheckStartGame(int countPlayers, int lobbyid)
        {
            if (countPlayers == ruls.RulesList[0])
            {
                StartGame(lobbyid);
                return "Новая игра начата";
            }

            return string.Format($"Ожидание новых игроков, не хватает {ruls.RulesList[0] - countPlayers}");
        }
    }
}
