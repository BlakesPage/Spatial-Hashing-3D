using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlakesHashGrid
{
    public interface ISpatialHash3D
    {
        Vector3 GetPosition();
    }

    public class HashGrid3D<T> where T : ISpatialHash3D
    {
        public float GridDimensions;
        public Vector3 CellSize;

        public int columnsX;
        public int rowsY;
        public int tubeZ;

        public Dictionary<int, List<T>> cells;
        public int CellCount;

        public HashGrid3D(int cols, int rows, int tubes, float dimensions)
        {
            GridDimensions = dimensions;
            columnsX = cols;
            rowsY = rows;
            tubeZ = tubes;

            CellSize.x = GridDimensions / columnsX;
            CellSize.y = GridDimensions / rowsY;
            CellSize.z = GridDimensions / tubeZ;

            this.cells = new Dictionary<int, List<T>>(columnsX * rowsY * tubeZ);

            for (int i = 0; i < cols * rows * tubes; i++)
            {
                this.cells.Add(i, new List<T>());
            }

            this.CellCount = cells.Count;
        }

        // add object to cell
        public void Insert(T obj)
        {

        }

        // get nearby objects in cell
        public List<T> GetNearby(T obj)
        {
            return null;
        }

        private int[] GetCellIDs(T obj)
        {



            return null;
        }

        // Better implementation of Floor, which boosts the floor performance greatly
        private const int _BIG_ENOUGH_INT = 16 * 1024;
        private const double _BIG_ENOUGH_FLOOR = _BIG_ENOUGH_INT + 0.0000;

        private static int FastFloor(float f)
        {
            return (int)(f + _BIG_ENOUGH_FLOOR) - _BIG_ENOUGH_INT;
        }
    }
}