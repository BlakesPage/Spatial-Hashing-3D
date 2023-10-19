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
    
    public int TotalCells;

    public List<SpatialHashObject> HashObjectList = new List<SpatialHashObject>();

    HashGrid3D<SpatialHashObject> grid;

    private void Awake()
    {
        grid = new HashGrid3D<SpatialHashObject>(rowsX, columnsY, tubeZ, GridDimensions.x, GridDimensions.y, GridDimensions.z);

        for (int i = 0; i < HashObjectList.Count; i++)
        {
            grid.Insert(HashObjectList[i]);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        foreach (SpatialHashObject t in HashObjectList)
        {
            grid.UpdateIndex(t);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if(grid != null)
        {
            foreach (Vector3 v in grid.cellPositions)
            {
                Gizmos.DrawWireCube(new Vector3(v.x * grid.CellSize.x, v.y * grid.CellSize.y, v.z * grid.CellSize.z) + 
                                    new Vector3(grid.CellSize.x / 2, grid.CellSize.y / 2, grid.CellSize.z / 2), grid.CellSize);
            }
        }
    }
}
