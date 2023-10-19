using System.Collections.Generic;
using UnityEngine;
using BlakesHashGrid;

public class GridMaker : MonoBehaviour
{
    [Header("Cell Size")]
    public Vector3 cellSize;

    [Header("Total Cells")]
    public int TotalCells;

    [Header("Gizmos")]
    public bool ShowGizmos;

    [Header("List of All Hash Objects")]
    public List<SpatialHashObject> HashObjectList = new List<SpatialHashObject>(); // this is not need just for testing

    HashGrid3D<SpatialHashObject> grid;

    private void Awake()
    {
        grid = new HashGrid3D<SpatialHashObject>(cellSize);

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

        TotalCells = (int)grid.CurrentCellCount;
    }

    public void OnDrawGizmos()
    {
        if(ShowGizmos == true)
        {
            Gizmos.color = Color.magenta;
            if (grid != null)
            {
                foreach (Vector3 v in grid.cellPositions)
                {
                    Gizmos.DrawWireCube(new Vector3(v.x * grid.CellSize.x, v.y * grid.CellSize.y, v.z * grid.CellSize.z) +
                                        new Vector3(grid.CellSize.x / 2, grid.CellSize.y / 2, grid.CellSize.z / 2), grid.CellSize);
                }
            }
        }
    }
}
