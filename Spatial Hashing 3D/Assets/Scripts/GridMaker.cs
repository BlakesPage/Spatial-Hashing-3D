using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlakesHashGrid;

public class GridMaker : MonoBehaviour
{
    public float GridDimensions;
    [HideInInspector] public Vector3 cellSize;

    public int columnsX;
    public int rowsY;
    public int tubeZ;

    private List<Vector3> gridPoints = new List<Vector3>();
    public int TotalCells;

    HashGrid3D<ISpatialHash3D> grid;

    private void Awake()
    {
        cellSize.x = GridDimensions / columnsX;
        cellSize.y = GridDimensions / rowsY;
        cellSize.z = GridDimensions / tubeZ;

        for (int x = 0; x < columnsX; x++)
        {
            for (int y = 0; y < rowsY; y++)
            {
                for (int z = 0; z < tubeZ; z++)
                {
                    gridPoints.Add(new Vector3(x * cellSize.x, y * cellSize.x, z * cellSize.z));
                }
            }
        }

        TotalCells = gridPoints.Count;

        grid = new HashGrid3D<ISpatialHash3D>(columnsX, rowsY, tubeZ, GridDimensions);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (var p in gridPoints)
        {
            Gizmos.DrawWireCube(p, cellSize);
        }
    }
}
