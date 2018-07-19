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
        public int rotate;
        public int userid;
        public CoordinateEvents UserCoordinate = new CoordinateEvents();
        public static void AddNewPlayer(int playerId, int userId, int lobbydId)
        {
            ParseJsonManager e = new ParseJsonManager();
            int h = e.GetMazeSize(lobbydId).x;
            int w = e.GetMazeSize(lobbydId).y;
            Random rnd = new Random();
            CoordinateEvents a = new CoordinateEvents();
            Player player = new Player
            {
                playerid = playerId,
                rotate = rnd.Next(1, 4),
                userid = userId,
                UserCoordinate = a.GeneraCoordinateEvents(lobbydId)
                
            };
        }

        public static List<int> GetPlayerInfo(int playerId)
        {

            List<int> a = null;

            return a;
        }
    }
}
