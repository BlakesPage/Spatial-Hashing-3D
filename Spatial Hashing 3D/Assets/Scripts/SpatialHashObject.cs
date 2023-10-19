using UnityEngine;

namespace BlakesSpatialHash
{
    public interface ISpatialHash3D
    {
        Vector3 GetPosition();
        uint Index { get; set; }
        bool Enabled { get; }
    }

    public class SpatialHashObject : MonoBehaviour, ISpatialHash3D
    {
        public Vector3 GetPosition() { return transform.position; }
        public uint Index { get; set; }
        public bool Enabled { get { return gameObject.activeSelf; } }
    }
}