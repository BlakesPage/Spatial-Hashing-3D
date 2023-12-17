using System.Collections.Generic;
using UnityEngine;

namespace BlakesSpatialHash
{
    public interface ISpatialHash3D
    {
        Vector3 GetPosition { get; }
        Vector3 GetLastPosition { get; set; }
        uint Index { get; set; }
        bool Enabled { get; }
    }

    public class SpatialHashObject : MonoBehaviour, ISpatialHash3D
    {
        public Vector3 GetPosition { get { return transform.position; } }
        public Vector3 GetLastPosition { get; set; }
        public uint Index { get; set; }
        public bool Enabled { get { return gameObject.activeSelf; } }
        public List<SpatialHashObject> Objects { get; set; } = new List<SpatialHashObject>();

        public bool DistanceCheck(Vector3 otherObj, float inRange)
        {
            if(Vector3.Distance(transform.position, otherObj) < inRange)
            {
                return true;
            }
            
            return false;
        }
    }
}