using System.Collections.Generic;
using UnityEngine;

namespace BlakesHashGrid
{
    public class HashGrid3D<T> where T : SpatialHashObject
    {
        public Vector3 GridDimensions;
        public Vector3 CellSize;

        public int rowsX;
        public int columnsY;
        public int tubeZ;

        public Dictionary<uint, List<T>> cells;
        public int CellCount;

        private static Vector3[] offsets;

        public HashGrid3D(int cols, int rows, int tubes, float dimensionsX, float dimensionsY, float dimensionsZ)
        {
            GridDimensions.x = dimensionsX;
            GridDimensions.y = dimensionsY;
            GridDimensions.z = dimensionsZ;
            rowsX = rows;
            columnsY = cols;
            tubeZ = tubes;

            CellSize.x = GridDimensions.x / rowsX;
            CellSize.y = GridDimensions.y / columnsY;
            CellSize.z = GridDimensions.z / tubeZ;
            CellSize /= 2;

            cells = new Dictionary<uint, List<T>>(rowsX * columnsY * tubeZ);

            for (uint i = 0; i < rowsX * columnsY * tubeZ; i++)
            {
                cells.Add(i, new List<T>());
            }

            CellCount = cells.Count;

            offsets = new Vector3[26];

            // bottom 9 of the 3x3x3
            offsets[0] = new Vector3(-CellSize.x, -CellSize.y, -CellSize.z);    // 1x1x1
            offsets[1] = new Vector3(-CellSize.x, -CellSize.y, 0);              // 1x1x2
            offsets[2] = new Vector3(-CellSize.x, -CellSize.y, CellSize.z);     // 1x1x3
            offsets[3] = new Vector3(0, -CellSize.y, -CellSize.z);              // 2x1x1
            offsets[4] = new Vector3(0, -CellSize.y, 0);                        // 2x1x2
            offsets[5] = new Vector3(0, -CellSize.y, CellSize.z);               // 2x1x3
            offsets[6] = new Vector3(CellSize.x, -CellSize.y, -CellSize.z);     // 3x1x1
            offsets[7] = new Vector3(CellSize.x, -CellSize.y, 0);               // 3x1x2
            offsets[8] = new Vector3(CellSize.x, -CellSize.y, CellSize.z);      // 3x1x3

            // middle  8 of the 3x3x3, excluding 2x2x2
            offsets[9] = new Vector3(-CellSize.x, 0, -CellSize.z);              // 1x2x1
            offsets[10] = new Vector3(-CellSize.x, 0, 0);                       // 1x2x2
            offsets[11] = new Vector3(-CellSize.x, 0, CellSize.z);              // 1x2x3
            offsets[12] = new Vector3(0, 0, -CellSize.z);                       // 2x2x1
            offsets[13] = new Vector3(0, 0, CellSize.z);                        // 2x2x3
            offsets[14] = new Vector3(CellSize.x, 0, -CellSize.z);              // 3x2x1
            offsets[15] = new Vector3(CellSize.x, 0, 0);                        // 3x2x2
            offsets[16] = new Vector3(CellSize.x, 0, CellSize.z);               // 3x2x3

            // top 9 of the 3x3x3
            offsets[17] = new Vector3(-CellSize.x, CellSize.y, -CellSize.z);    // 1x3x1
            offsets[18] = new Vector3(-CellSize.x, CellSize.y, 0);              // 1x3x2
            offsets[19] = new Vector3(-CellSize.x, CellSize.y, CellSize.z);     // 1x3x3
            offsets[20] = new Vector3(0, CellSize.y, -CellSize.z);              // 2x3x1
            offsets[21] = new Vector3(0, CellSize.y, 0);                        // 2x3x2
            offsets[22] = new Vector3(0, CellSize.y, CellSize.z);               // 2x3x3
            offsets[23] = new Vector3(CellSize.x, CellSize.y, -CellSize.z);     // 3x3x1
            offsets[24] = new Vector3(CellSize.x, CellSize.y, 0);               // 3x3x2
            offsets[25] = new Vector3(CellSize.x, CellSize.y, CellSize.z);      // 3x3x3
        }

        /// <summary>
        /// Initalise OBJECT into Spatial Hash Grid
        /// </summary>
        /// <param name="obj"></param>
        public void Insert(T obj)
        {
            uint index = obj.Index = GetIndexFromHash(HashCell(PositionToCellCoord(obj.GetPosition(), CellSize)));

            cells[index].Add(obj);
        }

        /// <summary>
        /// Returns all OBJECTS in Current and Touching Cells
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<T> GetNearby(T obj)
        {
            List<T> tempList = new List<T>();

            // Cache object values for performance
            Vector3 position = obj.GetPosition();
            uint index = obj.Index;

            // Get all objects in current cell & remove this obj
            tempList.AddRange(cells[index]);
            tempList.Remove(obj);

            //get all objects in surrounding cells
            foreach (Vector3 v in offsets)
            {
                uint tempIndex = GetIndexFromHash(HashCell(PositionToCellCoord(position + v, CellSize)));

                if (index == tempIndex) { continue; }

                tempList.AddRange(cells[tempIndex]);
            }

            return tempList;
        }

        /// <summary>
        /// Called once per frame Updates the Cell Index Value
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateIndex(T obj)
        {
            uint index = obj.Index;
                
            uint newIndex = GetIndex(obj.GetPosition());

            if(index != newIndex)
            { 
                cells[index].Remove(obj);
                cells[newIndex].Add(obj);
                obj.Index = newIndex;
            }
        }

        private Vector3Int PositionToCellCoord(Vector3 point, Vector3 cellSize)
        {
            // set a min and max Vec3 and detect if outside of bounds
            //int cellX = FastFloor(point.x / (cellSize.x / 2));
            //int cellY = FastFloor(point.y / (cellSize.y / 2));
            //int cellZ = FastFloor(point.z / (cellSize.z / 2));
            int cellX = FastFloor(point.x / cellSize.x);
            int cellY = FastFloor(point.y / cellSize.y);
            int cellZ = FastFloor(point.z / cellSize.z);
            return new Vector3Int(cellX, cellY, cellZ);
        }

        private uint HashCell(Vector3Int cellCoord)
        {
            var hash = Hash128.Compute(ref cellCoord);
            return (uint)hash.GetHashCode();
        }

        private uint GetIndexFromHash(uint num)
        {
            return (uint)(num % CellCount);
        }

        private uint GetIndex(Vector3 point)
        {
            return GetIndexFromHash(HashCell(PositionToCellCoord(point, CellSize)));
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