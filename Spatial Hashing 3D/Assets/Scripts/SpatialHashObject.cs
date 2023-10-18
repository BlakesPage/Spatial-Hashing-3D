using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlakesHashGrid;

public class SpatialHashObject : MonoBehaviour, ISpatialHash3D
{
    public uint Index { get; set; }
    public int Id { get; set; }
    public Vector3 GetPosition() { return transform.position; }
}
