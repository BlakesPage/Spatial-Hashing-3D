using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BlakesHashGrid
{
    public interface ISpatialHash3D
    {
        Vector3 GetPosition();
        uint Index { get; set; }
        int Id { get; set; } 
    }

    public class HashGrid3D<T> where T : ISpatialHash3D
    {
        public Vector3 GridDimensions;
        public Vector3 CellSize;

        public int rowsX;
        public int columnsY;
        public int tubeZ;

        //3D Coordinates of the down left corner of 
        //public float minX, minY, minZ;

        public Dictionary<uint, List<T>> cells;
        public int CellCount;

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

            cells = new Dictionary<uint, List<T>>(rowsX * columnsY * tubeZ);

            for (uint i = 0; i < rowsX * columnsY * tubeZ; i++)
            {
                cells.Add(i, new List<T>());
            }

            CellCount = cells.Count;
        }

        // Initalise obj into Spatial Hash Grid
        public void Insert(T obj)
        {
            uint index = obj.Index = GetIndexFromHash(HashCell(PositionToCellCoord(obj.GetPosition(), CellSize)));

            cells[index].Add(obj);
        }

        // get nearby objects in cell
        public List<T> GetNearby(T obj)
        {
            List<T> tempList = new List<T>();

            // Cache object values for performance
            uint index = obj.Index;
            int id = obj.Id;

            foreach (T t in cells[index]) 
            {
                if(t.Id != id)
                {
                    tempList.Add(t);
                }
            }

            // add extra detection for surrounding cells

            return tempList;
        }

        public void UpdateIndex(T obj)
        {
            uint index = obj.Index;
                
            uint newIndex = GetIndex(obj.GetPosition());

            if(index != newIndex)
            { 
                obj.Index = newIndex;
                cells[index].Remove(obj);
                Debug.Log(obj.Index);
                return;
            }
        }

        public void test(T obj)
        {
            obj.Index = GetIndexFromHash(HashCell(PositionToCellCoord(obj.GetPosition(), CellSize)));
        }

        public Vector3Int PositionToCellCoord(Vector3 point, Vector3 cellSize)
        {
            int cellX = FastFloor(point.x / cellSize.x);
            int cellY = FastFloor(point.y / cellSize.y);
            int cellZ = FastFloor(point.z / cellSize.z);
            return new Vector3Int(cellX, cellY, cellZ);
        }

        public uint HashCell(Vector3Int cellCoord)
        {
            var hash = Hash128.Compute(ref cellCoord);
            return (uint)hash.GetHashCode();
        }

        public uint GetIndexFromHash(uint num)
        {
            return (uint)(num % CellCount);
        }

        public uint GetIndex(Vector3 point)
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