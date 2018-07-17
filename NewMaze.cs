using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGenerator;

namespace ConsoleApplication1
{
    class NewMaze
    {
        public static string GetNewMaze()
        {
            ushort h, w;
            Random rnd = new Random();
            int tmp;
            tmp = rnd.Next(10, 20);
            h = (ushort) tmp;
            tmp = rnd.Next(10, 20);
            w = (ushort) tmp;
            Maze maze1 = new Maze(h, w);
            maze1.dumpMaze();

            List<UInt16[]> gt_output = maze1.GenerateTWMaze_GrowingTree();
            maze1.dumpMaze();

            Byte[,] blockmaze = maze1.LineToBlock();
            string s = "";
            for (UInt16 y = 0; y < blockmaze.GetLength(1); y++)
            {
                string xline = string.Empty;

                for (UInt16 x = 0; x < blockmaze.GetLength(0); x++)
                {
                    xline += ' ' + blockmaze[x, y].ToString();
                }

                s += string.Format("{1}", y, xline);
                //   File.Create("new_labirinth.dat").Close();
                //File.AppendAllText("new_labirinth.dat", string.Format("{1}", y, xline));

            }

            return s;
        }
    }
}
