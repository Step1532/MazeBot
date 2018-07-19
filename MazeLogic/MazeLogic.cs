using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGenerator.NewGame;
using MazeGenerator.TeleBot;
using MazeGenerator.Tools;
using Telegram.Bot;

namespace MazeGenerator.MazeLogic
{
    public class MazeLogic
    {
        public Answers a = new Answers();
        public Random rnd = new Random();
        //public readonly TelegramBotClient Bot;
        public Player player = new Player();
        public ParseJsonManager PJson = new ParseJsonManager();
        //public JsonManager MJson = new JsonManager();
        //public Rules ruls = new Rules();
        //public MazeLogic act = new MazeLogic();
        public bool[,] maze;
        public  bool TryMoveUp(int playerId, int gameid)
        {
            List<Player> players = new List<Player>();
            players = PJson.GetPlayersList(gameid);
            player = players.Find(e => player.playerid == playerId);
            maze = PJson.GetMazeMap(gameid);
            if (maze[player.UserCoordinate.x, player.UserCoordinate.y - 1] == false)
            {
                player.UserCoordinate.y--;
                return true;
            }
            else
            {
                return false;
            }
        }
        public  void Shoot(string Action, int playerId)
        {
;
        }
    }
}
