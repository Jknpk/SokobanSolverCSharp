using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanSolver
{
    public enum Direction { North, East, South, West, NoNeighbor};
    class PointOnMap
    {
        private int row;
        private int column;

        public int Row { get => row; set => row = value; }
        public int Column { get => column; set => column = value; }

        public FieldType FieldType{ get; set; }

        public int order;

        public PointOnMap(int x, int y, FieldType currentFieldType)
        {
            Row = x;
            Column = y;
            FieldType = currentFieldType;
        }



        public static bool operator ==(PointOnMap a, PointOnMap b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.row == b.row && a.column == b.column;
        }
        public static bool operator !=(PointOnMap a, PointOnMap b)
        {
            return a.row != b.row || a.column != b.column;
        }

        public override string ToString()
        {
            return "(" + row+ "|" + column + ")";
        }

        public static Direction DirectionOfNeighbor(Map map, PointOnMap a, PointOnMap b)
        {
            if (map.GetElementNorth(a) == b) return SokobanSolver.Direction.North;
            if (map.GetElementEast(a) == b) return SokobanSolver.Direction.East;
            if (map.GetElementSouth(a) == b) return SokobanSolver.Direction.South;
            if (map.GetElementWest(a) == b) return SokobanSolver.Direction.West;
            return SokobanSolver.Direction.NoNeighbor;
        }
    }


}
