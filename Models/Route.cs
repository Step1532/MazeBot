using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenerator.Models
{
    public enum Route
    {
        North,
        West,
        South,
        East
    }

    public static class RouteExtensions
    {
        public static Coordinate GetCoordinate(this Route route)
        {
            
            //TODO: реализовать нормально этот метод
            switch (route)
            {
                case Route.North: return new Coordinate(0, -1);
                //TODO: ну и дальше по аналогии
                //TODO: сделать такой же трюк для Coordinate
                //case Route.East: a = -1; b = 0; break;
                //case Route.South: a = 0; b = 1; break;
                //case Route.West: a = 0; b = 0; break;

            }

            return null;
        }
    }
}
