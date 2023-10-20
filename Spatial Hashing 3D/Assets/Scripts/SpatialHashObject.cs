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
    }
}