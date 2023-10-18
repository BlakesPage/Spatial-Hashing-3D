using System.Collections.Generic;
using UnityEngine;
using BlakesHashGrid;

public class GridMaker : MonoBehaviour
{
    [Header("Size")]
    public Vector3 GridDimensions;
    [HideInInspector] public Vector3 cellSize;

    [Header("Parts")]
    public int rowsX;
    public int columnsY;
    public int tubeZ;

    private List<Vector3> gridPoints = new List<Vector3>();
    public int TotalCells;

    public List<SpatialHashObject> HashObjectList = new List<SpatialHashObject>();

    HashGrid3D<SpatialHashObject> grid;

    private void Awake()
    {
        cellSize.x = GridDimensions.x / rowsX;
        cellSize.y = GridDimensions.y / columnsY;
        cellSize.z = GridDimensions.z / tubeZ;

        for (int x = 0; x < rowsX; x++)
        {
            for (int y = 0; y < columnsY; y++)
            {
                for (int z = 0; z < tubeZ; z++)
                {
                    gridPoints.Add(new Vector3(x * cellSize.x, y * cellSize.y, z * cellSize.z) - (GridDimensions / 2) + (cellSize / 2));
                }
            }
        }

        TotalCells = gridPoints.Count;

        grid = new HashGrid3D<SpatialHashObject>(rowsX, columnsY, tubeZ, GridDimensions.x, GridDimensions.y, GridDimensions.z);

        for (int i = 0; i < HashObjectList.Count; i++)
        {
            grid.Insert(HashObjectList[i]);
        }

        //Debug.Log(grid.GetNearby(HashObjectList[0]).Count);
    }

    void Start()
    {
        Debug.Log("Object 1 index: " + HashObjectList[0].Index);
    }

    void Update()
    {
        foreach (SpatialHashObject t in HashObjectList)
        {
            grid.UpdateIndex(t);
        }

        Debug.Log("Object 1 has: " + grid.GetNearby(HashObjectList[0]).Count + " Nearby.");
        Debug.Log("Object 2 index: " + HashObjectList[1].Index);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < gridPoints.Count; i++)
        {
            Gizmos.DrawWireCube(gridPoints[i], cellSize);
        }
    }
}
