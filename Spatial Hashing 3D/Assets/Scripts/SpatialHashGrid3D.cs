using System.Collections.Generic;
using UnityEngine;
using BlakesSpatialHash;

public class SpatialHashGrid3D : MonoBehaviour
{
    [Header("Cell Size")]
    public Vector3 cellSize;

    [Header("Use Spatial Hashing")]
    public bool SpatialHashing = false;

    [Header("Search Range")]
    public float inRange = 10;

    [Header("Total Cells")]
    public int TotalCells;

    [Header("Gizmos")]
    public bool ShowGizmosBoxes;
    public bool ShowGizmosLines;
    public bool ShowDebugs;

    [Header("List of All Hash Objects")]
    public List<SpatialHashObject> HashObjectList = new List<SpatialHashObject>(); // this is not needed just for testing

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

    void Update()
    {
        int counter = 0;

        if(SpatialHashing == true)
        {
            foreach (SpatialHashObject obj in HashObjectList)
            {
                obj.Objects = grid.UpdateObjectAndGetSurroudingObjects(obj);

                foreach (SpatialHashObject objects in obj.Objects)
                {
                    counter++;

                    if (obj.DistanceCheck(objects.GetPosition, inRange))
                    {
                        
                    }
                }
            }
            if(ShowDebugs == true)
            {
                Debug.Log("Spatial Hashing Checks Per Frame: " + counter);
            }

        }
        else
        {
            foreach (SpatialHashObject obj in HashObjectList)
            {
                foreach (SpatialHashObject checking in HashObjectList)
                {
                    if (obj == checking) { continue; }

                    counter++;

                    if (obj.DistanceCheck(checking.GetPosition, inRange))
                    {
                        
                    }
                }
            }

            if (ShowDebugs == true)
            {
                Debug.Log("All Objects Checks Per Frame: " + counter);
            }
        }
    }

    public void OnDrawGizmos()
    {
        if(ShowGizmosBoxes == true)
        {
            Gizmos.color = Color.magenta;
            if (grid != null)
            {
                if(SpatialHashing == true)
                {
                    int count = grid.cellPositions.Count;

                    for (int i = 0; i < count; i++)
                    {
                        Vector3 v = grid.cellPositions[i];

                        Gizmos.DrawWireCube(new Vector3(v.x * cellSize.x, v.y * cellSize.y, v.z * cellSize.z) +
                                           new Vector3(cellSize.x / 2, cellSize.y / 2, cellSize.z / 2), cellSize);
                    }
                }
                else
                {
                    grid.cellPositions.Clear();
                }
            }
        }

        if(ShowGizmosLines)
        {
            if(grid != null)
            {
                if (SpatialHashing == true)
                {
                    Gizmos.color = Color.red;

                    foreach (SpatialHashObject obj in HashObjectList)
                    {
                        obj.Objects = grid.UpdateObjectAndGetSurroudingObjects(obj);

                        foreach (SpatialHashObject objects in obj.Objects)
                        {
                            Vector3 pos = objects.GetPosition;

                            if (obj.DistanceCheck(pos, inRange))
                            {
                                Gizmos.DrawLine(obj.GetPosition, pos);
                            }
                        }
                    }
                }
            }
        }
    }
}
