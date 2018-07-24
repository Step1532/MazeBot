using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenerator.Models
{
    public class Treasure
    {
        public Coordinate Position { get; set; }
        public bool IsTrue  { get; set; }

        public Treasure(Coordinate position, bool istrue)
        {
            Position = position;
            IsTrue = istrue;
        }
    }
}
