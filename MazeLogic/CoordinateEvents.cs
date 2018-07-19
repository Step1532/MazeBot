using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using MazeGenerator.Tools;

namespace MazeGenerator.MazeLogic
{
    public class CoordinateEvents
    {
        public int x { get; set; }
        public int y { get; set; }

        public CoordinateEvents GeneraCoordinateEvents(int gameId)
        {
            CoordinateEvents e = new CoordinateEvents();
            ParseJsonManager a = new ParseJsonManager();
            Random rnd =new Random();
            bool[,] mazeMap = a.GetMazeMap(gameId);
            e = a.GetMazeSize(gameId);
            int x, y;
            for (int i = 0; i < 1000; i++)
            {
                x = rnd.Next(0, e.x);
                y = rnd.Next(0, e.y);
                if (!mazeMap[x, y])
                {
                    e.x = x;
                    e.y = y;
                    return e;
                }
            }
            Console.WriteLine("Error");
            return e;
        }
    }
}
