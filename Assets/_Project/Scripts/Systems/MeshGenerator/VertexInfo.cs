using UnityEngine;

namespace ProjectMud.Systems.MeshGenerator
{
    public struct VertexInfo
    {
        public int Index;
        public Vector3 Position;
        public Vector2 uv;
        public VertexInfo(int index, Vector3 position, Vector2 uv)
        {
            Index = index;
            Position = position;
            this.uv = uv;
        }
        public VertexInfo(int index, Vector3 position)
        {
            Index = index;
            Position = position;
            uv = position;
        }
    }
}
