using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestSpatialHash : MonoBehaviour
{
    public int TotalCells;
    public float radius;

    //private void Awake()
    //{
    //    GetKeyFromHash(HashCell(new Vector3(150, 400, 2)));
    //}

    public void UpdateSpatialLookup(Vector3[] points)
    {

    }

    public void FindCell(Vector3 samplePoint)
    {
        Vector3Int centre = PositionToCellCoord(samplePoint, radius);
        float sqrRadius = radius * radius;

        int key = GetKeyFromHash(HashCell(centre));
    }

    public Vector3Int PositionToCellCoord(Vector3 point, float radius)
    {
        int cellX = (int)(point.x / radius);
        int cellY = (int)(point.y / radius);
        int cellZ = (int)(point.z / radius);
        return new Vector3Int(cellX, cellY, cellZ);
    }

    public Hash128 HashCell(Vector3Int cellCoord)
    {
        return Hash128.Compute(ref cellCoord);
    }

    public int GetKeyFromHash(Hash128 hash)
    {
        return hash.GetHashCode() % TotalCells;
    }
}
