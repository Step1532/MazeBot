using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MazeGenerator.MazeLogic;
using MazeGenerator.Tools;

namespace MazeGenerator.NewGame
{
    public class Player
    {
        public int playerid;
        public CoordinateEvents rotate = new CoordinateEvents();
        public int userid;
        public CoordinateEvents UserCoordinate = new CoordinateEvents();
        public  JsonManager json = new JsonManager();
        public ParseJsonManager pjson = new ParseJsonManager();
        public void AddNewPlayer(int playerId, int userId, int lobbydId)
        { 
            ParseJsonManager e = new ParseJsonManager();
            int h = e.GetMazeEventses(lobbydId)[0].x;
            int w = e.GetMazeEventses(lobbydId)[0].y;
            Random rnd = new Random();
            CoordinateEvents a = new CoordinateEvents();
            a.x = rnd.Next(-1, 1);
            a.y = rnd.Next(-1, 1);
            Player player = new Player
            {
                playerid = playerId,
                rotate = a,
                userid = userId,
                UserCoordinate = a.GeneraCoordinateEvents(lobbydId)              
            };

            json.WritePlayersToJson(player, lobbydId);
            Console.WriteLine(player.playerid + " " + player.rotate.x + " " + player.rotate.y + " " + player.userid + " " + player.UserCoordinate.x + player.UserCoordinate.y);
        }

        public static List<int> GetPlayerInfo(int playerId)
        {

            List<int> a = null;

            return a;
        }
    }
}
