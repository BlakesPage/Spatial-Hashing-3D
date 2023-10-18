using System.Collections;
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

    public List<Transform> positionsTest = new List<Transform>();
    private List<ISpatialHash3D> objList = new List<ISpatialHash3D>();

    HashGrid3D<ISpatialHash3D> grid;

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
                    gridPoints.Add(new Vector3(x * cellSize.x, y * cellSize.y, z * cellSize.z));
                }
            }
        }

        TotalCells = gridPoints.Count;

        grid = new HashGrid3D<ISpatialHash3D>(rowsX, columnsY, tubeZ, GridDimensions.x, GridDimensions.y, GridDimensions.z);

        foreach (Transform t in positionsTest)
        {
            ISpatialHash3D temp = t.GetComponent<ISpatialHash3D>();
            objList.Add(temp);
            grid.Insert(temp);
        }

        for (int i = 0; i < positionsTest.Count; i++)
        {
            ISpatialHash3D temp = positionsTest[i].GetComponent<ISpatialHash3D>();
            temp.Id = i;
            objList.Add(temp);
            grid.Insert(temp);
        }

        foreach (ISpatialHash3D t in objList)
        {
            Debug.Log(grid.cells[t.Index].Count);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        foreach (ISpatialHash3D t in objList)
        {
            grid.UpdateIndex(t);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        for (int i = 0; i < gridPoints.Count; i++)
        {
            Gizmos.DrawWireCube(gridPoints[i], cellSize);
        }
    }

    // Better implementation of Floor, which boosts the floor performance greatly
    private const int _BIG_ENOUGH_INT = 16 * 1024;
    private const double _BIG_ENOUGH_FLOOR = _BIG_ENOUGH_INT + 0.0000;

    private static int FastFloor(float f)
    {
        return (int)(f + _BIG_ENOUGH_FLOOR) - _BIG_ENOUGH_INT;
    }
}
