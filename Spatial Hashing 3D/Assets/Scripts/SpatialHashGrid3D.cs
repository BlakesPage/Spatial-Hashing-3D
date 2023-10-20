using System.Collections.Generic;
using UnityEngine;
using BlakesSpatialHash;

public class SpatialHashGrid3D : MonoBehaviour
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

        int count = HashObjectList.Count;

        for (int i = 0; i < count; i++)
        {
            grid.Insert(HashObjectList[i]);
        }

        TotalCells = (int)grid.CellCountMax;
    }

    void Start()
    {
        
    }

    void Update()
    {
        int count = HashObjectList.Count;

        for (int i = 0; i < count; i++)
        {
            grid.UpdateObjectAndGetSurroudingObjects(HashObjectList[i]);
        }
    }

    public void OnDrawGizmos()
    {
        if(ShowGizmos == true)
        {
            Gizmos.color = Color.magenta;
            if (grid != null)
            {
                int count = grid.cellPositions.Count;

                for (int i = 0; i < count; i++)
                {
                    Vector3 v = grid.cellPositions[i];

                    Gizmos.DrawWireCube(new Vector3(v.x * cellSize.x, v.y * cellSize.y, v.z * cellSize.z) +
                                       new Vector3(cellSize.x / 2, cellSize.y / 2, cellSize.z / 2), cellSize);
                }
            }
        }
    }
}
